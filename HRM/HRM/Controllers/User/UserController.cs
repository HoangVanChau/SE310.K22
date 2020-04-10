using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using HRM.Constants;
using HRM.Extensions;
using HRM.Helpers;
using HRM.Models.Cores;
using HRM.Models.Requests;
using HRM.Models.Responses.Bases;
using HRM.Repositories.Counter;
using HRM.Repositories.User;
using HRM.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Controllers.User
{
    [ApiController]
    [Route("[controller]")]
    public class UserController: ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly ICounterRepository _counterRepo;
        private readonly IAuthService _authService;

        public UserController(IUserRepository userRepo, IAuthService authService, ICounterRepository counterRepo)
        {
            _userRepo = userRepo;
            _authService = authService;
            _counterRepo = counterRepo;
        }
        
        [HttpPost]
        [Route("/api/user/register")]
        public async Task<JsonResult> Register([FromBody] UserRegisterRequest registerData)
        {
            var newUserUuid = Guid.NewGuid().ToString();
            var newEmployeeId = await _counterRepo.GetNextCounterValue(Collections.UserCollection);
            var newUser = new Models.Cores.User
            {
                UserId = newUserUuid,
                FullName = registerData.FullName,
                Email = registerData.Email,
                EmployeeId = newEmployeeId,
                HashPassword = _authService.HashPassword(registerData.Password),
                Role = Roles.Member,
                PhoneNumber = registerData.PhoneNumber,
                UserName = registerData.UserName,
                DateOfBirth = DateTime.ParseExact(registerData.DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture)
            };

            try
            {
                await _userRepo.InsertOne(newUser);
            }
            catch (Exception e)
            {
                return ExceptionHelper.HandleError(e);
            }

            var responseData = new Dictionary<String, Object>
            {
                {"user", newUser.WithoutPassword()},
                {"accessToken", _authService.GenerateAccessToken(newUser.UserId, newUser.Role)},
                {"refreshToken", _authService.GenerateRefreshToken(newUser.UserId)}
            };

            return new OkResponse(new BaseResponse<Object>()
            {
                Code = HttpStatusCode.OK,
                Data = responseData,
                Message = "Đăng kí thành công!",
                Error = null
            });
        }

        [Authorize(Roles = Roles.SuperAdmin)]
        [HttpGet]
        [Route("/api/GetAllUsers")]
        public async Task<JsonResult> GetAllUsers()
        {
            return new OkResponse(await _userRepo.GetAllDocument());
        }
        
    }
}