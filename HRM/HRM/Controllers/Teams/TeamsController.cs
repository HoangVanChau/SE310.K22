using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRM.Constants;
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
        [AllowAllSystemUser]
        public async Task<JsonResult> GetTeams()
        {
            return new OkResponse(await _teamRepo.GetAllTeams());
        }
        
        [HttpPost]
        [RoleWithSuperAdmin(Constants.Roles.Director)]
        public async Task<JsonResult> CreateTeam([FromBody] Team newTeam)
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
                MembersId = newTeam.MembersId ?? new List<string>(),
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
        
        [HttpGet("{teamId}")]
        [AllowAllSystemUser]
        public async Task<JsonResult> GetTeam(string teamId)
        {
            var team = await _teamRepo.GetTeamByTeamId(teamId);
            if (team == null)
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Team không tồn tại!"
                });
            }
            return new OkResponse(team);
        }

        [HttpPatch("{teamId}")]
        [RoleWithSuperAdmin(Constants.Roles.Hr)]
        public async Task<JsonResult> PatchTeam(string teamId, [FromBody] Team updateData)
        {
            var team = await _teamRepo.GetTeamByTeamId(teamId);
            if (team == null)
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Team không tồn tại!"
                });
            }
            
            var updateDefine = Builders<Team>.Update.CurrentDate(x => x.LastModifyDate);
            
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

        [HttpDelete("{teamId}")]
        [RoleWithSuperAdmin(Constants.Roles.Director)]
        public async Task<JsonResult> DeleteTeam(string teamId)
        {
            var team = await _teamRepo.GetTeamByTeamId(teamId);
            if (team == null)
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Team không tồn tại!"
                });
            }
            
            var result = await _teamRepo.DeleteTeam(teamId);
            if (result)
            {
                return new OkResponse(new
                {
                    Message = "Xóa Team thành công!"
                });
            }
            else
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Lỗi xảy ra khi xóa team!"
                });
            }
        }

        [HttpGet("{teamId}/members")]
        [AllowAllSystemUser]
        public async Task<JsonResult> GetMembersOfTeam(string teamId)
        {
            var team = await _teamRepo.GetTeamByTeamId(teamId);
            if (team == null)
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Team không tồn tại!"
                });
            }
            
            return new OkResponse(await _teamRepo.GetMembersOfTeam(teamId));
        }

        [HttpPost("{teamId}/members")]
        [RoleWithSuperAdmin(Constants.Roles.Manager)]
        public async Task<JsonResult> AddMemberToTeam(string teamId,[FromBody] User newUser)
        {
            var targetTeam = await _teamRepo.GetTeamByTeamId(teamId);
            if (targetTeam == null)
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Team không tồn tại!"
                });
            }
            
            var newMemberData = await _userRepo.FindUserByUserId(newUser.UserId);
            if (newMemberData.Role != Constants.Roles.Employee)
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Nhân viên viên mới của team phải có role Employee!"
                });
            }
            
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
            
            var currentUserId = User.Identity.GetId();
            var currentUser = await _userRepo.FindUserByUserId(currentUserId);

            if (targetTeam.Leaders.FirstOrDefault()?.UserId != currentUserId && currentUser.Role != Constants.Roles.SuperAdmin)
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
        
        [HttpDelete("{teamId}/members/{memberId}")]
        [RoleWithSuperAdmin(Constants.Roles.Manager)]
        public async Task<JsonResult> DeleteMemberOfTeam(string teamId, string memberId)
        {
            var currentUserId = User.Identity.GetId();
            var currentUser = await _userRepo.FindUserByUserId(currentUserId);
            var targetTeam = await _teamRepo.GetTeamByTeamId(teamId);

            if (targetTeam == null)
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Team không tồn tại!"
                });
            }
            
            if (targetTeam.Leaders.FirstOrDefault()?.UserId != currentUserId && currentUser.Role != Constants.Roles.SuperAdmin)
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Leader của team mới có quyền xóa thành viên"
                });
            }
            
            var result = await _teamRepo.DeleteUserFromTeam(teamId, memberId);
            if (result)
            {
                return new OkResponse(new
                {
                    Message = "Đã xóa nhân viên ra khỏi team!"
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
        
        [HttpPatch("{teamId}/leader")]
        [RoleWithSuperAdmin(Constants.Roles.Director)]
        public async Task<JsonResult> ChangeLeaderOfTeam(string teamId, [FromBody] User newLeader)
        {
            var team = await _teamRepo.GetTeamByTeamId(teamId);
            if (team == null)
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Team không tồn tại!"
                });
            }
            
            if (newLeader.UserId == null)
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Thông tin leader mới bị lỗi"
                });
            }

            var newLeaderData = await _userRepo.FindUserByUserId(newLeader.UserId);
            if (newLeaderData.Role != Constants.Roles.Manager)
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "User phải có Role là Manager mới được làm Lead của team",
                    Data = newLeaderData
                });
            }

            var checkExistInAnotherTeam = await _teamRepo.FindLeaderExistInAnyTeam(newLeader.UserId);
            if (checkExistInAnotherTeam != null)
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Leader này đã có mặt ở team khác",
                    Data = checkExistInAnotherTeam
                });
            }
            
            var updateDefine = Builders<Team>.Update.CurrentDate(x => x.LastModifyDate);
            updateDefine = updateDefine.Set(x => x.LeaderId, newLeader.UserId);

            var result = await _teamRepo.UpdateTeamInfoByTeamId(teamId, updateDefine);
            if (result)
            {
                return new OkResponse(new
                {
                    Message = "Thay đổi leader thành công!"
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
    }
}