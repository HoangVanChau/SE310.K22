using System;
using System.Linq;
using System.Threading.Tasks;
using HRM.Extensions;
using HRM.Models.Cores;
using HRM.Models.Responses.Bases;
using HRM.Repositories.Team;
using HRM.Repositories.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace HRM.Controllers.Teams
{
    [Route("api/[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamRepository _teamRepo;
        private readonly IUserRepository _userRepo;

        public TeamsController(ITeamRepository teamRepo, IUserRepository userRepo)
        {
            _teamRepo = teamRepo;
            _userRepo = userRepo;
        }

        [HttpGet]
        public async Task<JsonResult> Get()
        {
            return new OkResponse(await _teamRepo.GetAllTeams());
        }
        
        [HttpGet("{teamId}")]
        public async Task<JsonResult> Get(string teamId)
        {
            return new OkResponse(await _teamRepo.GetTeamByTeamId(teamId));
        }
        
        [HttpPatch("{teamId}")]
        [Authorize(Roles = Constants.Roles.Hr)]
        public async Task<JsonResult> Patch(string teamId, [FromBody] Team updateData)
        {
            var updateDefine = Builders<Team>.Update.CurrentDate(x => x.LastModifyDate);
            if (updateData.LeaderId != null)
            {
                updateDefine = updateDefine.Set(x => x.LeaderId, updateData.LeaderId);
            }
            
            if (updateData.TeamName != null)
            {
                updateDefine = updateDefine.Set(x => x.TeamName, updateData.TeamName);
            }
            
            if (updateData.TeamAvatarImageId != null)
            {
                updateDefine = updateDefine.Set(x => x.TeamAvatarImageId, updateData.TeamAvatarImageId);
            }

            var result = await _teamRepo.UpdateTeamInfoByTeamId(teamId, updateDefine);
            if (result)
            {
                return new OkResponse(new
                {
                    Message = "Sửa thông tin team thành công"
                });
            }
            else
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Lỗi khi thay đổi thông tin team!"
                });
            }
        }
        
        [HttpPost("{teamId}")]
        [Authorize(Roles = Constants.Roles.Manager)]
        public async Task<JsonResult> Post(string teamId,[FromBody] User newUser)
        {
            var currentUserId = User.Identity.GetId();
            var targetTeam = await _teamRepo.GetTeamByTeamId(teamId);

            var checkExistTeam = _teamRepo.FindUserExistInAnyTeam(newUser.UserId);
            if (checkExistTeam != null && checkExistTeam.Count > 0)
            {
                if (checkExistTeam.Find(x => x.MembersId.Contains(newUser.UserId)).TeamId == teamId)
                {
                    return new BadRequestResponse(new ErrorData
                    {
                        Message = "User đã tồn tại ở chính Team này!",
                        Data = checkExistTeam
                    });
                }
                return new BadRequestResponse(new ErrorData
                {
                    Message = "User đã tồn tại ở Team khác!",
                    Data = checkExistTeam
                });
            }

            if (targetTeam.Leaders.FirstOrDefault()?.UserId != currentUserId)
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Leader của team mới có quyền thêm thành viên mới"
                });
            }
            
            var result = await _teamRepo.AddNewUserToTeam(teamId, newUser.UserId);
            if (result)
            {
                return new OkResponse(new
                {
                    Message = "Thêm nhân viên vào Team thành công"
                });
            }
            else
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Lỗi khi thay đổi thông tin team!"
                });
            }
        }
        
        [HttpDelete("{teamId}")]
        [Authorize(Roles = Constants.Roles.Manager)]
        public async Task<JsonResult> Delete(string teamId, [FromBody] User removeUser)
        {
            var currentUserId = User.Identity.GetId();
            var targetTeam = await _teamRepo.GetTeamByTeamId(teamId);
            
            if (targetTeam.Leaders.FirstOrDefault()?.UserId != currentUserId)
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Leader của team mới có quyền xóa thành viên"
                });
            }
            
            var result = await _teamRepo.DeleteUserFromTeam(teamId, removeUser.UserId);
            if (result)
            {
                return new OkResponse(new
                {
                    Message = "Đã xóa User ra khỏi team!"
                });
            }
            else
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Lỗi khi thay đổi thông tin team!"
                });
            }
        }

        [HttpPost]
        [Authorize(Roles = Constants.Roles.Director)]
        public async Task<JsonResult> Post([FromBody] Team newTeam)
        {
            var leader = await _userRepo.FindUserByUserId(newTeam.LeaderId);
            if (leader == null)
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Không tìm thấy Leader!"
                });
            }

            if (leader.Role != Constants.Roles.Manager)
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Leader phải có chức vụ Manager!"
                });
            }

            var insertData = new Team
            {
                TeamId = Guid.NewGuid().ToString(),
                TeamName = newTeam.TeamName,
                CreatedDate = DateTime.Now,
                LeaderId = newTeam.LeaderId,
                MembersId = newTeam.MembersId,
                TeamAvatarImageId = newTeam.TeamAvatarImageId
            };

            try
            {
                await _teamRepo.InsertOne(insertData);
                return new OkResponse(insertData);
            }
            catch (Exception e)
            {
                return Helpers.ExceptionHelper.HandleError(e);
            }
        }
    }
}