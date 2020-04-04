using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using HRM.Helpers;
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

        public ImageController(IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }
        
        [Route("/image/upload")]
        [HttpPost]
        public async Task<JsonResult> UploadImage(IFormFile file)
        {
            if (WriterHelper.CheckIfImageFile(file))
            {
                var imageName = await WriteFile(file);
                return new JsonResult(imageName);
            }
            else
            {
                return new JsonResult(null)
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        }
        
        public async Task<string> WriteFile(IFormFile file)
        {
            string fileName;
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                fileName = Guid.NewGuid() + extension; 
                var path = Path.Combine(_hostEnvironment.ContentRootPath, "Resources/Images", fileName);

                await using var bits = new FileStream(path, FileMode.Create);
                await file.CopyToAsync(bits);
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return fileName;
        }
    }
}