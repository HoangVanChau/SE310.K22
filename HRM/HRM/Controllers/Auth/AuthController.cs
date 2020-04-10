using System;
using System.Net;
using System.Threading.Tasks;
using HRM.Constants;
using HRM.Models;
using HRM.Models.Cores;
using HRM.Repositories.User;
using HRM.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HRM.Extensions;
using HRM.Models.Bases;
using HRM.Models.Requests;
using HRM.Models.Responses;
using HRM.Models.Responses.Bases;

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
        public async Task<JsonResult> Login([FromBody] AuthRequest authRequest)
        {
            var user = await _userRepo.FindUserByUserName(authRequest.UserName);
            if (user == null)
            {
                return new BadRequestResponse(new BaseResponse<Object>
                {
                    Code = HttpStatusCode.BadRequest,
                    Message = "Không tìm thấy tài khoản!",
                    Data = null,
                    Error = "Cannot find user with username"
                });
            }

            if (user.HashPassword == _authService.HashPassword(authRequest.Password))
            {
                return new OkResponse(new BaseResponse<AuthResponse>()
                {
                    Data = new AuthResponse
                    {
                        AccessToken = _authService.GenerateAccessToken(user.UserId, user.Role),
                        RefreshToken = _authService.GenerateRefreshToken(user.UserId)
                    },
                    Error = null,
                    Message = "Đăng nhập thành công!"
                });
            }

            return new BadRequestResponse(new BaseResponse<Object>
            {
                Code = HttpStatusCode.BadRequest,
                Message = "Mật khẩu không đúng. Vui lòng thử lại!",
                Data = null,
                Error = "Wrong password"
            });
        }
        
        [HttpPost]
        [Route("/api/auth/refresh")]
        public async Task<JsonResult> RefreshToken([FromBody] RefreshTokenRequest requestData)
        {
            var payLoad = _authService.VerifyToken(requestData.RefreshToken);
            if (payLoad == null)
            {
                return new UnauthorizedResponse(null);
            }

            var userId = payLoad["unique_name"].ToString();
            var user = await _userRepo.FindUserByUserId(userId);

            if (user == null)
            {
                return new BadRequestResponse(new BaseResponse<BaseModel>()
                {
                    Code = HttpStatusCode.BadRequest,
                    Data = null,
                    Error = "Cannot find user by user Id in token",
                    Message = "Lỗi nhận diện User!"
                });
            }

            return new OkResponse(new AuthResponse()
            {
                AccessToken = _authService.GenerateAccessToken(user.UserId, user.Role),
                RefreshToken = requestData.RefreshToken
            });
        }
        
        [Authorize(Roles = Roles.Member)]
        [HttpGet]
        [Route("/api/auth/verify")]
        public async Task<JsonResult> VerifyToken([FromHeader] string token)
        {
            var userId = User.Identity.GetId();
            var user = await _userRepo.FindUserByUserId(userId);
            
            return new OkResponse(new BaseResponse<VerifyUserResponse>()
            {
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
                Message = "Thành công!"
            });
        }
    }
}