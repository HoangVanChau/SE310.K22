using System;
using System.Threading.Tasks;
using HRM.Constants;
using HRM.Models.Cores;
using HRM.Models.Responses.Bases;
using HRM.Repositories.Utils.CompanyInfo;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Controllers.Utils
{
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyInfoRepository _companyInfoRepo;

        public CompanyController(ICompanyInfoRepository companyInfoRepo)
        {
            _companyInfoRepo = companyInfoRepo;
        }

        [HttpGet]
        [AllowAllSystemUser]
        public async Task<JsonResult> Get()
        {
            return new OkResponse(await _companyInfoRepo.GetCompanyInfo());
        }

        [HttpPost]
        [RoleWithSuperAdmin(Constants.Roles.Hr)]
        public async Task<JsonResult> Post([FromBody] CompanyInfo body)
        {
            var checkExistCompanyInfo = await _companyInfoRepo.GetCompanyInfo();
            if (checkExistCompanyInfo != null)
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Đã tồn tại thông tin công ty, không thể tạo mới!"
                });
            }
            body.ActiveDate = DateTime.Parse(body.RawActiveDate);
            body.LicenseDate = DateTime.Parse(body.RawLicenseDate);
            try
            {
                await _companyInfoRepo.InsertOne(body);
                return new OkResponse(new
                {
                    Message = "Tạo thông tin công ty thành công!"
                });
            }
            catch (Exception e)
            {
                return Helpers.ExceptionHelper.HandleError(e);
            }
        }
        
        [HttpPatch]
        [RoleWithSuperAdmin(Constants.Roles.Hr)]
        public async Task<JsonResult> Patch([FromBody] CompanyInfo body)
        {
            var result = await _companyInfoRepo.UpdateCompanyInfo(body);
            if (result)
            {
                return new OkResponse(new
                {
                    Message = "Update thông tin công ty thành công!"
                });
            }
            else
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Update thông tin công ty thất bại!"
                });
            }
        }
    }
}