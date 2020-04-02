using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using HRM.Constants;
using HRM.Models;
using HRM.Models.Base;
using HRM.Models.Cores;
using HRM.Models.Requests;
using HRM.Repositories.User;
using HRM.Services.Auth;
using HRM.Services.MongoDB;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IAuthService _authService;

        public UserController(IUserRepository userRepo, IAuthService authService)
        {
            _userRepo = userRepo;
            _authService = authService;
        }
        
        [AllowAnonymous]
        [HttpPost]
        [Route("/api/user/register")]
        public JsonResult Register([FromBody] UserRegisterRequest registerData)
        {
            var firstRole = new List<String>();
            firstRole.Add("Member");

            var newUserId = Guid.NewGuid().ToString();
            
            var newUser = new User
            {
                UserId = newUserId,
                FullName = registerData.FullName,
                Email = registerData.Email,
                HashPassword = _authService.HashPassword(registerData.Password),
                Roles = firstRole,
                PhoneNumber = registerData.PhoneNumber,
                UserName = registerData.UserName,
                DateOfBirth = DateTime.ParseExact(registerData.DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture)
            };
            
            _userRepo.InsertOne(newUser);
            
            var responseUser = new Dictionary<String, Object>();
            responseUser.Add("userId", newUser.UserId);
            responseUser.Add("employeeId", newUser.EmployeeId);
            
            var responseData = new Dictionary<String, Object>();
            responseData.Add("user", responseUser);
            responseData.Add("accessToken", _authService.GenerateAccessToken(newUser.UserId));
            responseData.Add("refreshToken", _authService.GenerateRefreshToken(newUser.UserId));
            
            return new JsonResult(new BaseResponse<Object>()
            {
                Code = HttpStatusCode.OK,
                Data = responseData,
                Message = "Đăng kí thành công!",
                Error = null
            });
        }

        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        [Route("/api/GetAllUsers")]
        public JsonResult GetAllUsers()
        {
            return new JsonResult(_userRepo.GetAllDocument());
        }
    }
}