using System.Threading.Tasks;
using HRM.Constants;
using HRM.Models.Cores;
using HRM.Models.Responses.Bases;
using HRM.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Controllers.Utils
{
    [Route("api/[controller]")]
    public class HolidaysController : ControllerBase
    {
        private readonly IHolidayRepository _holidayRepo;

        public HolidaysController(IHolidayRepository holidayRepo)
        {
            _holidayRepo = holidayRepo;
        }

        [HttpGet]
        [AllowAllSystemUser]
        public async Task<JsonResult> Get()
        {
            return new OkResponse(await _holidayRepo.GetAllDocument());
        }

        [HttpPost]
        [RoleWithSuperAdmin(Constants.Roles.Hr)]
        public async Task<JsonResult> Post([FromBody] Holiday body)
        {
            var id = await _holidayRepo.InsertOne(body);
            return new OkResponse(new
            {
                Message = "Thêm ngày lễ thành công!",
                Id = id
            });
        }
        
        [HttpDelete]
        [RoleWithSuperAdmin(Constants.Roles.Hr)]
        [Route("{holidayId}")]
        public async Task<JsonResult> Delete(string holidayId)
        {
            var result = await _holidayRepo.DeleteOneById(holidayId);
            if (result)
            {
                return new OkResponse(new
                {
                    Messagge = "Xóa ngày lễ thành công!"
                });
            }
            else
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Xóa ngày nghỉ thất bại!"
                });
            }
        }
    }
}