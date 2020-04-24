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

namespace HRM.Controllers.Users
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
            var newUser = new Models.Cores.UserAuth
            {
                UserId = newUserUuid,
                FullName = registerData.FullName,
                Email = registerData.Email,
                EmployeeId = newEmployeeId,
                HashPassword = _authService.HashPassword(registerData.Password),
                Address = registerData.Address,
                Role = Constants.Roles.Member,
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
                {"user", newUser},
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
            var updateDefine = Builders<User>.Update.CurrentDate(x => x.LastModifyDate);
            
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
        
        [HttpPatch("{userId}")]
        [Authorize(Roles = Constants.Roles.Hr)]
        public async Task<JsonResult> Patch(string userId, [FromBody] UserModifyRequest updateData)
        {
            var updateDefine = Builders<User>.Update.Set(u => u.Email, updateData.Email);
            
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
            return new OkResponse(currentUser);
        }
        
        [HttpGet("{userId}")]
        [AllowAllSystemUser]
        public async Task<JsonResult> Get(string userId)
        {
            var user = await _userRepo.FindUserByUserId(userId);
            return new OkResponse(user);
        }

        [HttpDelete("{userId}")]
        [Authorize(Roles = Constants.Roles.SuperAdmin)]
        public async Task<JsonResult> Delete(string userId)
        {
            var result = await _userRepo.DeleteUserByUserid(userId);
            if (result)
            {
                return new OkResponse(new
                {
                    Message = "Xóa thành công User!"
                });
            }
            else
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Lỗi khi xóa User!"
                });
            }
        }
        
        //for testing...... must remove 
        [HttpGet]
        [Route("getAllUser")]
        public async Task<JsonResult> GetAllUsers()
        {
            return new OkResponse(await _userRepo.GetAllDocument());
        }
        
        //for testing...... must remove 
        [HttpGet]
        [Route("getSuperAdmin")]
        public async Task<JsonResult> GetSuperAdmin()
        {
            var supperAdmin = await _userRepo.GetUsersByRole(Constants.Roles.SuperAdmin);
            if (supperAdmin.Count == 0)
            {
                var newSupperAdmin = new Models.Cores.UserAuth
                {
                    UserId = Guid.NewGuid().ToString(),
                    HashPassword = _authService.HashPassword("123456789"),
                    Role =  Constants.Roles.SuperAdmin,
                    UserName = "SupperAdmin"
                };
                await _userRepo.InsertOne(newSupperAdmin);
                
                return new OkResponse(newSupperAdmin);
            }
            else
            {
                return new OkResponse(supperAdmin);
            }
        }
        
    }
}