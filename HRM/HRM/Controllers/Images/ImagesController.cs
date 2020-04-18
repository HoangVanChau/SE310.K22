using System;
using System.IO;
using System.Threading.Tasks;
using HRM.Helpers;
using HRM.Models.Cores;
using HRM.Models.Responses.Bases;
using HRM.Repositories.Image;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace HRM.Controllers.Images
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController: ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }
        
        [HttpPost]
        public async Task<JsonResult> Post(IFormFile file)
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

        [HttpGet]
        public async Task<FileContentResult> Get(String id)
        { 
            var imageFile = await _imageRepository.FindFileById(id);
            byte[] imageBytes = Convert.FromBase64String(imageFile.RawBinary);
            
            return new FileContentResult(imageBytes, "image/jpeg");
        }

        private async Task<string> WriteFileToMongoDb(IFormFile file)
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