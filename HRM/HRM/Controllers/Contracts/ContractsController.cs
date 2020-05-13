using System;
using System.Threading.Tasks;
using HRM.Constants;
using HRM.Helpers;
using HRM.Models.Cores;
using HRM.Models.Responses.Bases;
using HRM.Repositories.Contract;
using HRM.Repositories.Position;
using HRM.Repositories.Team;
using HRM.Repositories.User;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace HRM.Controllers.Contracts
{
    [Route("api/[controller]")]
    public class ContractsController : ControllerBase
    {
        private readonly IContractRepository _contractRepo;
        private readonly IUserRepository _userRepo;
        private readonly ITeamRepository _teamRepo;
        private readonly IPositionRepository _positionRepo;

        public ContractsController(
            IContractRepository contractRepo, 
            IUserRepository userRepo, 
            ITeamRepository teamRepo,
            IPositionRepository positionRepo)
        {
            _contractRepo = contractRepo;
            _userRepo = userRepo;
            _teamRepo = teamRepo;
            _positionRepo = positionRepo;
        }

        [HttpGet]
        [AllowAllSystemUser]
        public async Task<JsonResult> GetPositions()
        {
            var result = await _contractRepo.GetAllDocument();
            return new OkResponse(result);
        }

        [HttpPost]
        [RoleWithSuperAdmin(Constants.Roles.Hr)]
        public async Task<JsonResult> CreateContract([FromBody] Contract newContract)
        {
            if (!ValidateUtils.ValidateNewContract(newContract))
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Thông tin hợp đồng không hợp lệ!"
                });
            }
            
            var targetUser = await _userRepo.FindUserByUserId(newContract.UserId);
            if (targetUser == null)
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Không tìm thấy User!"
                });
            }

            var targetTeam = await _teamRepo.GetTeamByTeamId(newContract.TeamId);
            if (targetTeam == null)
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Không tìm thấy Team!"
                });
            }
            
            var targetPosition = await _positionRepo.GetPositionById(newContract.PositionId);
            if (targetPosition == null)
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Không tìm thấy Position!"
                });
            }
            
            newContract.ContractId = Guid.NewGuid().ToString();
            try
            {
                await _contractRepo.InsertOne(newContract);
                return new OkResponse(newContract);
            }
            catch (Exception e)
            {
                return ExceptionHelper.HandleError(e);
            }
        }

        [Route("{contractId}")]
        [HttpGet]
        [RoleWithSuperAdmin(Constants.Roles.Hr)]
        public async Task<JsonResult> GetContract(string contractId)
        {
            var position = await _contractRepo.GetByContractId(contractId);
            return new OkResponse(position);
        }

        [Route("{contractId}")]
        [HttpPatch]
        [RoleWithSuperAdmin(Constants.Roles.Hr)]
        public async Task<JsonResult> PatchContract(string contractId, [FromBody] Contract data)
        {
            var contract = await _contractRepo.GetByContractId(contractId);
            if (contract.DisableDate != null)
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Không thể sửa hợp đồng đã bị hủy",
                    Data = contract
                });
            }
            var now = DateTime.Now;
            var updateDefine = Builders<Contract>.Update.Set(x => x.LastModifyDate, now);

            if (data.ContractName != null)
            {
                updateDefine = updateDefine.Set(x => x.ContractName, data.ContractName);
            }

            if (data.Active != null)
            {
                updateDefine = updateDefine.Set(x => x.Active, data.Active);
                if (data.Active == false)
                {
                    updateDefine = updateDefine.Set(x => x.DisableDate, now);
                }
            }

            try
            {
                var result = await _contractRepo.UpdateByContractId(contractId, updateDefine);
                return new OkResponse(new
                {
                    Message = "Sửa thông tin position thành công",
                    Data = result
                });
            }
            catch (Exception e)
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Lỗi khi sửa thông tin Position",
                    Data = e
                });
            }
        }
    }
}