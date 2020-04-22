using System;
using System.Threading.Tasks;
using HRM.Models.Cores;
using HRM.Models.Responses.Bases;
using HRM.Repositories.Team;
using HRM.Repositories.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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