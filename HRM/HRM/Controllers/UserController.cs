using System.Linq;
using HRM.Models;
using HRM.Repositories.User;
using HRM.Services.MongoDB;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HRM.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController: ControllerBase
    {

        private readonly IUserRepository _userRepo;

        public UserController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }
        
        [HttpGet]
        [Route("/api/test")]
        public JsonResult test()
        {
            _userRepo.InsertOne(new User()
            {
                Email = "bacvip999@gmail.com",
                FullName = "Bac Nguyen xuan",
                PhoneNumber = "0123456789"
            });
            
            return new JsonResult(_userRepo.UpdateOneById("5e771ba727ffc65e51a5de20", 
                Builders<User>.Update
                    .Set(user => user.FullName, "Phan Trong ba")
                )
            );
        }

        [HttpGet]
        [Route("/api/GetAllUsers")]
        public JsonResult GetAllUsers()
        {
            return new JsonResult(_userRepo.GetAllDocument());
        }
    }
}