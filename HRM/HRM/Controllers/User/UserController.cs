using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using HRM.Constants;
using HRM.Extensions;
using HRM.Models.Cores;
using HRM.Models.Requests;
using HRM.Repositories.User;
using HRM.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Controllers.User
{
    [Authorize]
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
            var firstRoles = Role.FirstMemberRoles();
            var newUserId = Guid.NewGuid().ToString();
            var newUser = new Models.Cores.User
            {
                UserId = newUserId,
                FullName = registerData.FullName,
                Email = registerData.Email,
                HashPassword = _authService.HashPassword(registerData.Password),
                Roles = firstRoles,
                PhoneNumber = registerData.PhoneNumber,
                UserName = registerData.UserName,
                DateOfBirth = DateTime.ParseExact(registerData.DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture)
            };
            
            _userRepo.InsertOne(newUser);
            
            var responseData = new Dictionary<String, Object>();
            responseData.Add("user", newUser.WithoutPassword());
            responseData.Add("accessToken", _authService.GenerateAccessToken(newUser.UserId, newUser.Roles));
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
        
        [Authorize(Roles = Role.OwnInfoModify)]
        [HttpGet]
        [Route("/api/OwnInfoModify")]
        public JsonResult TestOwnInfoModify()
        {
            return new JsonResult(User.Identity.GetId());
        }
        
        [Authorize(Roles = Role.OwnInfoRead)]
        [HttpGet]
        [Route("/api/OwnInfoRead")]
        public JsonResult OwnInfoRead()
        {
            return new JsonResult(User.Identity.GetId());
        }
    }
}