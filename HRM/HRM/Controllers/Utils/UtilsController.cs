using System.Linq;
using System.Threading.Tasks;
using HRM.Models.Cores;
using HRM.Models.Responses.Bases;
using HRM.Repositories.Utils;
using HRM.Repositories.Utils.Address;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Controllers.Utils
{
    public class UtilsController : ControllerBase
    {
        private readonly IAddressRepository _addressRepo;

        public UtilsController(IAddressRepository addressRepo)
        {
            _addressRepo = addressRepo;
        }

        [Route("api/provinces")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> GetProvince()
        {
            var provinces = await _addressRepo.GetProvinces();
            return new OkResponse(provinces);
        }
        
        [Route("api/districts")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> GetDistricts([FromQuery] string provinceId)
        {
            var districts = await _addressRepo.GetDistricts(provinceId);
            if (districts.Count == 0)
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Không tìm thấy danh sách huyện tương ứng!"
                });
            }
            return new OkResponse(districts);
        }
        
        [Route("api/wards")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> GetWards([FromQuery] string districtId)
        {
            var wards = await _addressRepo.GetWards(districtId);
            if (wards.Count == 0)
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Không tìm thấy danh sách xã tương ứng!"
                });
            }
            return new OkResponse(wards);
        }
    }
}