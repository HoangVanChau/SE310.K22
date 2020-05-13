using System;
using System.Threading.Tasks;
using HRM.Constants;
using HRM.Helpers;
using HRM.Models.Cores;
using HRM.Models.Responses.Bases;
using HRM.Repositories.Position;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace HRM.Controllers.Positions
{    
    [Route("api/[controller]")]
    public class PositionsController : ControllerBase
    {
        private readonly IPositionRepository _positionRepo;

        public PositionsController(IPositionRepository positionRepo)
        {
            _positionRepo = positionRepo;
        }
        
        [HttpGet]
        [AllowAllSystemUser]
        public async Task<JsonResult> GetPositions()
        {
            var result = await _positionRepo.GetAllDocument();
            return new OkResponse(result);
        }
        
        [HttpPost]
        [RoleWithSuperAdmin(Constants.Roles.Hr)]
        public async Task<JsonResult> CreatePosition([FromBody] Position newPosition)
        {
            newPosition.PositionId = Guid.NewGuid().ToString();
            try
            {
                await _positionRepo.InsertOne(newPosition);
                return new OkResponse(newPosition);
            }
            catch (Exception e)
            {
                return ExceptionHelper.HandleError(e);
            }
        }

        [Route("{positionId}")]
        [HttpGet]
        [RoleWithSuperAdmin(Constants.Roles.Hr)]
        public async Task<JsonResult> GetPosition(string positionId)
        {
            var position = await _positionRepo.GetPositionById(positionId);
            return new OkResponse(position);
        }
        
        [Route("{positionId}")]
        [HttpPatch]
        [RoleWithSuperAdmin(Constants.Roles.Hr)]
        public async Task<JsonResult> PatchPosition(string positionId, [FromBody] Position data)
        {
            var now = DateTime.Now;
            var updateDefine = Builders<Position>.Update.Set(x => x.LastModifyDate, now);
            if (data.Description != null)
            {
                updateDefine = updateDefine.Set(x => x.Description, data.Description);
            }
            if (data.PositionName != null)
            {
                updateDefine = updateDefine.Set(x => x.PositionName, data.PositionName);
            }
            if (data.BaseMonthSalary != null)
            {
                updateDefine = updateDefine.Set(x => x.BaseMonthSalary, data.BaseMonthSalary);
            }
            if (data.BaseHourSalary != null)
            {
                updateDefine = updateDefine.Set(x => x.BaseHourSalary, data.BaseHourSalary);
            }
            if (data.BaseOtSalaryPerHour != null)
            {
                updateDefine = updateDefine.Set(x => x.BaseOtSalaryPerHour, data.BaseOtSalaryPerHour);
            }
            if (data.BaseDateOff != null)
            {
                updateDefine = updateDefine.Set(x => x.BaseDateOff, data.BaseDateOff);
            }
            if (data.BaseLateMoney != null)
            {
                updateDefine = updateDefine.Set(x => x.BaseLateMoney, data.BaseLateMoney);
            }

            try
            {
                var result = await _positionRepo.UpdateByPositionId(positionId, updateDefine);
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