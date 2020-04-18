using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using HRM.Helpers;
using HRM.Models.Cores;
using HRM.Models.Responses.Bases;
using HRM.Repositories.Image;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;

namespace HRM.Controllers.Image
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController: ControllerBase
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IImageRepository _imageRepository;

        public ImageController(IHostEnvironment hostEnvironment, IImageRepository imageRepository)
        {
            _hostEnvironment = hostEnvironment;
            _imageRepository = imageRepository;
        }
        
        [Route("/image/upload")]
        [HttpPost]
        public async Task<JsonResult> UploadImage(IFormFile file)
        {
            if (WriterHelper.CheckIfImageFile(file))
            {
                var imageName = await WriteFileToMongoDb(file);
                return new OkResponse(imageName);
            }

            return new BadRequestResponse(new ErrorData
            {
                Message = "Định dạng không hợp lệ, chỉ chấp nhận JPEG/PNG"
            });
        }

        [Route("/image")]
        [HttpGet]
        public async Task<FileContentResult> GetImage(String id)
        { 
            var imageFile = await _imageRepository.FindFileById(id);
            byte[] imageBytes = Convert.FromBase64String(imageFile.RawBinary);
            
            return new FileContentResult(imageBytes, "image/jpeg");
        }

        public async Task<string> WriteFileToMongoDb(IFormFile file)
        {
            ObjectId fileName;
            await using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                var s = Convert.ToBase64String(fileBytes);
                
                var imageFile = new ImageFile
                {
                    RawBinary = s
                };
                fileName = await _imageRepository.SaveFile(imageFile);
            }

            return fileName.ToString();
        }
    }
}