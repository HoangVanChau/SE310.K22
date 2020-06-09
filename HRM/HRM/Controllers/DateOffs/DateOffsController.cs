using System.Linq;
using System.Threading.Tasks;
using HRM.Constants;
using HRM.Extensions;
using HRM.Models.Cores;
using HRM.Models.QueryParams;
using HRM.Models.Responses.Bases;
using HRM.Repositories.DateOff;
using HRM.Repositories.Team;
using HRM.Repositories.User;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace HRM.Controllers.DateOffs
{
    [Route("api/[controller]")]
    public class DateOffsController : ControllerBase
    {
        private readonly IDateOffRepository _dateOffRepo;
        private readonly ITeamRepository _teamRepo;
        private readonly IUserRepository _userRepo;
        public DateOffsController(IDateOffRepository dateOffRepo, ITeamRepository teamRepo, IUserRepository userRepo)
        {
            _dateOffRepo = dateOffRepo;
            _teamRepo = teamRepo;
            _userRepo = userRepo;
        }
        
        [AllowAllSystemUser]
        [HttpPost]
        public async Task<JsonResult> RequestDateOff([FromBody] DateOff data)
        {
            var currentUserId = User.Identity.GetId();
            var userData = await _userRepo.FindUserByUserId(currentUserId);
            if (userData.Role == Constants.Roles.Manager)
            {
                var currentTeam = await _teamRepo.FindLeaderExistInAnyTeam(currentUserId);
                data.TeamId = currentTeam.TeamId;
                //Auto approve for manager request off
                data.Status = Constants.DateOffStatus.Approve;
            }
            else
            {
                var currentTeam = _teamRepo.FindUserExistInAnyTeam(currentUserId);
                data.TeamId = currentTeam.FirstOrDefault()?.TeamId;
                data.Status = DateOffStatus.PendingApprove;
            }
            data.UserId = currentUserId;
            await _dateOffRepo.InsertOne(data);
            return new OkResponse(new
            {
                Message = "Yêu cầu ngày phép thành công!"
            });
        }
        
        [AllowAllSystemUser]
        [HttpGet]
        public async Task<JsonResult> GetDateOffs([FromQuery] DateOffQuery queryParams)
        {
            var queryGtDate = queryParams?.FromDate != null
                ? Builders<DateOff>.Filter.Gte(x => x.Date, queryParams.FromDate)
                : FilterDefinition<DateOff>.Empty;
            
            var queryLtDate = queryParams?.ToDate != null
                ? Builders<DateOff>.Filter.Lte(x => x.Date, queryParams.ToDate)
                : FilterDefinition<DateOff>.Empty;
            
            var queryUserId = queryParams?.UserId != null
                ? Builders<DateOff>.Filter.Eq(x => x.UserId, queryParams.UserId)
                : FilterDefinition<DateOff>.Empty;
            
            var queryStatus = queryParams?.Status != null
                ? Builders<DateOff>.Filter.Eq(x => x.Status.Value, queryParams.Status)
                : FilterDefinition<DateOff>.Empty;
            
            var queryTeamId = queryParams?.TeamId != null
                ? Builders<DateOff>.Filter.Eq(x => x.TeamId, queryParams.TeamId)
                : FilterDefinition<DateOff>.Empty;

            var totalFilter = queryGtDate & queryLtDate & queryUserId & queryStatus & queryTeamId;
            var result = await _dateOffRepo.QueryDateOffs(totalFilter);
            return new OkResponse(result);
        }

        [AllowAllSystemUser]
        [HttpGet]
        [Route("{dateOffId}")]
        public async Task<JsonResult> GetDateOff(string dateOffId)
        {
            var filter = Builders<DateOff>.Filter.Eq(x => x.Id, dateOffId);
            var result = await _dateOffRepo.QueryDateOff(filter);
            if (result == null)
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Không tìm thấy yêu cầu nghỉ phép!"
                });
            }
            return new OkResponse(result);
        }
        
        [AllowAllSystemUser]
        [HttpDelete]
        [Route("{dateOffId}")]
        public async Task<JsonResult> CancelDateOff(string dateOffId)
        {
            var currentUserId = User.Identity.GetId();
            var dateOffData = await _dateOffRepo.FindFirstById(dateOffId);
            if (dateOffData == null)
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Không tìm thấy yêu cầu ngày nghỉ!"
                });
            }

            if (dateOffData.UserId != currentUserId)
            {
                return new UnauthorizedResponse(new ErrorData
                {
                    Message = "Ngày nghỉ không phải của bạn!"
                });
            }

            if (dateOffData.Status == DateOffStatus.Approve)
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Không thể hủy ngày nghỉ đã được Approve!"
                });
            } 

            await _dateOffRepo.DeleteOneById(dateOffId);
            return new OkResponse(new
            {
                Message = "Xóa ngày nghỉ thành công!"
            });
        }
        
        [RoleWithSuperAdmin(Constants.Roles.Manager)]
        [HttpPut]
        [Route("{dateOffId}")]
        public async Task<JsonResult> ApproveDateOff(string dateOffId, [FromBody] DateOff bodyParams)
        {
            var currentLeader = User.Identity.GetId();
            var dateOffData = await _dateOffRepo.FindFirstById(dateOffId);
            if (dateOffData == null)
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Không tìm thấy ngày nghỉ!"
                });
            }
            var targetTeam = _teamRepo.FindUserExistInAnyTeam(dateOffData.UserId).FirstOrDefault();
            if (targetTeam?.LeaderId != currentLeader)
            {
                return new UnauthorizedResponse(new ErrorData
                {
                    Message = "Chỉ có leader mới có quyền approve phép!"
                });
            }
            var updateDefine = Builders<DateOff>.Update.Set(x => x.IsApprove, bodyParams.IsApprove);
            if (bodyParams.IsApprove == false)
            {
                updateDefine = updateDefine.Set(x => x.Status, DateOffStatus.Reject);
                updateDefine = updateDefine.Set(x => x.RejectReason, bodyParams.RejectReason);
            }
            else
            {
                updateDefine = updateDefine.Set(x => x.Status, DateOffStatus.Approve);
            }
            await _dateOffRepo.UpdateOneById(dateOffId, updateDefine);
            return new OkResponse(new
            {
                Message = "Thay đổi thành công!"
            });
        }
        
        [RoleWithSuperAdmin(Constants.Roles.Manager)]
        [HttpGet]
        [Route("team/{teamId}")]
        public async Task<JsonResult> GetDateOffsOfTeam(string teamId)
        {
            var currentLeaderId = User.Identity.GetId();
            var currentTeam = await _teamRepo.FindLeaderExistInAnyTeam(currentLeaderId);
            var filter = Builders<DateOff>.Filter.Where(x => currentTeam.MembersId.Contains(x.UserId));
            var listDateOff = await _dateOffRepo.QueryDateOffs(filter);
            return new OkResponse(listDateOff);
        }
    }
}