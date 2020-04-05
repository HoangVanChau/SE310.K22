using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using HRM.Helpers;
using HRM.Models.Cores;
using HRM.Repositories.Image;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

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
                return new JsonResult(imageName);
            }
            else
            {
                return new JsonResult(new BaseResponse<Object>
                {
                    Message = "Định dạng không hợp lệ, chỉ chấp nhận JPEG/PNG"
                })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        }

        [Route("/image")]
        [HttpGet]
        public async Task<FileContentResult> GetImage(String id)
        { 
            var imageFile = _imageRepository.FindFileById(id);
            byte[] imageBytes = Convert.FromBase64String(imageFile.RawBinary);
            
            return new FileContentResult(imageBytes, "image/jpeg");
        }
        
        // public async Task<string> WriteFile(IFormFile file)
        // {
        //     string fileName;
        //     try
        //     {
        //         var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
        //         fileName = Guid.NewGuid() + extension; 
        //         var path = Path.Combine(_hostEnvironment.ContentRootPath, "Resources/Images", fileName);
        //
        //         await using var bits = new FileStream(path, FileMode.Create);
        //         await file.CopyToAsync(bits);
        //     }
        //     catch (Exception e)
        //     {
        //         return e.Message;
        //     }
        //
        //     return fileName;
        // }

        public async Task<string> WriteFileToMongoDb(IFormFile file)
        {
            string  fileName;
            await using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                string s = Convert.ToBase64String(fileBytes);
                
                var imageFile = new ImageFile()
                {
                    RawBinary = s
                };
                fileName = _imageRepository.SaveFile(imageFile).ToString();
            }

            return fileName;
        }
    }
}