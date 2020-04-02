using System;
using System.Net;
using HRM.Models;
using HRM.Models.Cores;
using HRM.Repositories.User;
using HRM.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Controllers.Auth
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepo;

        public AuthController(IAuthService authService, IUserRepository userRepo)
        {
            _authService = authService;
            _userRepo = userRepo;
        }

        [HttpPost]
        [Route("/auth/login")]
        public JsonResult Login([FromBody] AuthRequest authRequest)
        {
            var user = _userRepo.FindUserByUserName(authRequest.UserName);
            if (user == null)
            {
                return new JsonResult(new BaseResponse<Object>
                {
                    Code = HttpStatusCode.BadRequest,
                    Message = "Không tìm thấy tài khoản!",
                    Data = null,
                    Error = "Cannot find user with username"
                });
            }
            else
            {
                if (user.HashPassword == _authService.HashPassword(authRequest.Password))
                {
                    return new JsonResult(new BaseResponse<AuthResponse>()
                    {
                        Code = HttpStatusCode.OK,
                        Data = new AuthResponse()
                        {
                            AccessToken = _authService.GenerateAccessToken(user.UserId),
                            RefreshToken = _authService.GenerateRefreshToken(user.UserId)
                        },
                        Error = null,
                        Message = "Đăng nhập thành công!"
                    });
                }
                else
                {
                    return new JsonResult(new BaseResponse<Object>
                    {
                        Code = HttpStatusCode.BadRequest,
                        Message = "Mật khẩu không đúng. Vui lòng thử lại!",
                        Data = null,
                        Error = "Wrong password"
                    });
                }
            }
        }
        
        [HttpGet]
        [Route("/auth/verify")]
        public JsonResult VerifyToken([FromHeader] string token)
        {
            var userId = _authService.VerifyToken(token);
            if (userId != null)
            {
                var user = _userRepo.FindUserByUserId(userId);
                return new JsonResult(new BaseResponse<VerifyUserResponse>()
                {
                    Code = HttpStatusCode.OK,
                    Data = new VerifyUserResponse()
                    {
                        UserId = user.UserId,
                        Address = user.Address,
                        Email = user.Email,
                        EmployeeId = user.EmployeeId,
                        FullName = user.FullName,
                        PhoneNumber = user.PhoneNumber,
                        Roles = user.Roles,
                        UserName = user.UserName
                    },
                    Message = "Thành công!",
                    Error = null
                });
            } else
            {
                return new JsonResult(new BaseResponse<Object>()
                {
                    Code = HttpStatusCode.Unauthorized,
                    Message = "Lỗi xác thực Token",
                    Data = null,
                    Error = "Unauthorized"
                });
            }
        }
    }
}