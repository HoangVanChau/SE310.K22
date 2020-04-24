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
using HRM.Repositories.AuthRepository;

namespace HRM.Controllers.Auth
{
    [ApiController]
    public class AuthController: ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IAuthRepository _authUserRepo;

        public AuthController(IAuthService authService, IAuthRepository authUserRepo)
        {
            _authService = authService;
            _authUserRepo = authUserRepo;
        }

        [HttpPost]
        [Route("/api/auth/login")]
        public async Task<JsonResult> Login([FromBody] AuthRequest authRequest)
        {
            var user = await _authUserRepo.FindUserAuthByUserName(authRequest.UserName);
            if (user == null)
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Không tìm thấy tài khoản!",
                    Data = null,
                });
            }

            if (user.HashPassword == _authService.HashPassword(authRequest.Password))
            {
                return new OkResponse(new AuthResponse
                {
                    AccessToken = _authService.GenerateAccessToken(user.UserId, user.Role),
                    RefreshToken = _authService.GenerateRefreshToken(user.UserId)
                });
            }

            return new BadRequestResponse(new ErrorData
            {
                Message = "Mật khẩu không đúng. Vui lòng thử lại!",
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
            var user = await _authUserRepo.FindUserAuthByUserId(userId);

            if (user == null)
            {
                return new BadRequestResponse(new ErrorData
                {
                    Data = null,
                    Message = "Lỗi nhận diện User!"
                });
            }

            return new OkResponse(new AuthResponse()
            {
                AccessToken = _authService.GenerateAccessToken(user.UserId, user.Role),
                RefreshToken = requestData.RefreshToken
            });
        }
        
        [AllowAllSystemUser]
        [HttpGet]
        [Route("/api/auth/verify")]
        public async Task<JsonResult> VerifyToken([FromHeader] string token)
        {
            var userId = User.Identity.GetId();
            var user = await _authUserRepo.FindUserAuthByUserId(userId);

            return new OkResponse(user.WithoutPassword());
        }
    }
}