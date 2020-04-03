using System;
using System.Net;
using HRM.Constants;
using HRM.Models;
using HRM.Models.Cores;
using HRM.Repositories.User;
using HRM.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HRM.Extensions;
using HRM.Models.Base;
using HRM.Models.Requests;

namespace HRM.Controllers.Auth
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController: ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepo;

        public AuthController(IAuthService authService, IUserRepository userRepo)
        {
            _authService = authService;
            _userRepo = userRepo;
        }

        [HttpPost]
        [Route("/api/auth/login")]
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
                            AccessToken = _authService.GenerateAccessToken(user.UserId, user.Role),
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
        
        [HttpPost]
        [Route("/api/auth/refresh")]
        public JsonResult RefreshToken([FromBody] RefreshTokenRequest requestData)
        {
            var payLoad = _authService.VerifyToken(requestData.RefreshToken);
            if (payLoad == null)
            {
                return new JsonResult(null)
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized
                };
            }
            else
            {
                var userId = payLoad["unique_name"].ToString();
                var user = _userRepo.FindUserByUserId(userId);

                if (user == null)
                {
                    return new JsonResult(new BaseResponse<BaseModel>()
                    {
                        Code = HttpStatusCode.BadRequest,
                        Data = null,
                        Error = "Cannot find user by user in token",
                        Message = "Lỗi hệ thống!"
                    })
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest
                    };
                }
                else
                {
                    return new JsonResult(new AuthResponse()
                    {
                        AccessToken = _authService.GenerateAccessToken(user.UserId, user.Role),
                        RefreshToken = requestData.RefreshToken
                    });
                }
            }
        }
        
        [Authorize(Roles = Roles.Member)]
        [HttpGet]
        [Route("/api/auth/verify")]
        public JsonResult VerifyToken([FromHeader] string token)
        {
            var userId = User.Identity.GetId();
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
                    Role = user.Role,
                    UserName = user.UserName
                },
                Message = "Thành công!",
                Error = null
            });
        }
    }
}