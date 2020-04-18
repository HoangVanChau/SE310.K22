using System;
using System.Collections.Generic;
using System.Globalization;
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
using MongoDB.Driver;

namespace HRM.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController: ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly ICounterRepository _counterRepo;
        private readonly IAuthService _authService;

        public UsersController(IUserRepository userRepo, IAuthService authService, ICounterRepository counterRepo)
        {
            _userRepo = userRepo;
            _authService = authService;
            _counterRepo = counterRepo;
        }
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> Post([FromBody] UserRegisterRequest registerData)
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
                Address = registerData.Address,
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

            return new OkResponse(new ErrorData
            {
                Data = responseData,
                Message = "Đăng kí thành công!",
            });
        }

        [HttpPatch]
        [AllowAllSystemUser]
        public async Task<JsonResult> Patch([FromBody] UserModifyRequest updateData)
        {
            var userId = User.Identity.GetId();
            var updateDefine = Builders<Models.Cores.User>.Update.Set(u => u.Email, updateData.Email);
            
            if (updateData.UserName != null)
            {
                updateDefine = updateDefine.Set(u => u.UserName, updateData.UserName);
            }
            if (updateData.Email != null)
            {
                updateDefine = updateDefine.Set(u => u.Email, updateData.Email);
            }

            if (updateData.FullName != null)
            {
                updateDefine = updateDefine.Set(u => u.FullName, updateData.FullName);
            }

            if (updateData.PhoneNumber != null)
            {
                updateDefine = updateDefine.Set(u => u.PhoneNumber, updateData.PhoneNumber);
            }
            
            if (updateData.Address != null)
            {
                updateDefine = updateDefine.Set(u => u.Address, updateData.Address);
            }

            if (updateData.DateOfBirth != null)
            {
                var newDoB = DateTime.Parse(updateData.DateOfBirth);
                updateDefine = updateDefine.Set(u => u.DateOfBirth, newDoB); 
            }

            try
            {
                var result = await _userRepo.UpdateUserByUserId(userId, updateDefine);
                if (result)
                {
                    var updatedUser = await _userRepo.FindUserByUserId(userId);
                    return new OkResponse(updatedUser);
                }
                else
                {
                    return new BadRequestResponse(new ErrorData
                    {
                        Message = "Update không thành công!"
                    });
                }
            }
            catch (Exception e)
            {
                return ExceptionHelper.HandleError(e);
            }
        }

        [HttpGet]
        [AllowAllSystemUser]
        public async Task<JsonResult> Get()
        {
            var userId = User.Identity.GetId();
            var currentUser = await _userRepo.FindUserByUserId(userId);
            return new OkResponse(currentUser.WithoutPassword());
        }
        
        //for testing...... must remove 
        [HttpGet]
        [Route("getAllUser")]
        public async Task<JsonResult> GetAllUsers()
        {
            return new OkResponse(await _userRepo.GetAllDocument());
        }
        
    }
}