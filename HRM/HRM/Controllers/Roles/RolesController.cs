using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HRM.Constants;
using HRM.Models.Cores;
using HRM.Models.Requests;
using HRM.Models.Responses.Bases;
using HRM.Repositories.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace HRM.Controllers.Roles
{
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public RolesController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult Get()
        {
            return new OkResponse(Constants.Roles.AllRoles());
        }

        [HttpPost]
        [RoleWithSuperAdmin(Constants.Roles.Hr)]
        public async Task<JsonResult> Post([FromBody] ChangeRoleRequest data)
        {
            if (!CheckCorrectRole(data.NewRole))
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Sai Role"
                });
            }
            var user = await _userRepo.FindUserByUserId(data.UserId);
            if (user == null)
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Không tìm thấy User"
                });
            }

            var updateBuilder = Builders<User>.Update.Set(u => u.Role, data.NewRole);
            try
            {
                var result = await _userRepo.UpdateUserByUserId(data.UserId, updateBuilder);
                if (result)
                {
                    return new OkResponse(new
                    {
                        Message = "Thay đổi role thành công!"
                    });
                }
                else
                {
                    return new BadRequestResponse(new ErrorData
                    {
                        Message = "Lỗi khi update role"
                    });
                }
            }
            catch (Exception e)
            {
                return Helpers.ExceptionHelper.HandleError(e);
            }
            
        }

        private bool CheckCorrectRole(string role)
        {
            return role != Constants.Roles.SuperAdmin && Constants.Roles.AllRoles().Contains(role);
        }
    }
}