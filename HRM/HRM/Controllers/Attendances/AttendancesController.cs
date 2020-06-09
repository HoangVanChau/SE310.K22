using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HRM.Constants;
using HRM.Helpers;
using HRM.Models;
using HRM.Models.Cores;
using HRM.Models.QueryParams;
using HRM.Models.Responses.Bases;
using HRM.Repositories.Attendance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using OfficeOpenXml;

namespace HRM.Controllers.Attendances
{
    [Route("api/[Controller]")]
    public class AttendancesController : ControllerBase
    {
        private readonly IAttendanceRepository _attendanceRepo;

        public AttendancesController(IAttendanceRepository attendanceRepo)
        {
            _attendanceRepo = attendanceRepo;
        }

        [HttpGet]
        [AllowAllSystemUser]
        public async Task<JsonResult> GetListAttendance([FromQuery] PagingParams pagingParams, [FromQuery] AttendanceQuery queryParams)
        {
            var queryGtDate = queryParams?.FromDate != null
                ? Builders<Attendance>.Filter.Gte(x => x.Unique.Date, queryParams.FromDate)
                : FilterDefinition<Attendance>.Empty;
            var queryLtDate = queryParams?.ToDate != null
                ? Builders<Attendance>.Filter.Lte(x => x.Unique.Date, queryParams.ToDate)
                : FilterDefinition<Attendance>.Empty;
            var queryEmployeeId = queryParams?.EmployeeId != null
                ? Builders<Attendance>.Filter.Eq(x => x.Unique.EmployeeId, queryParams.EmployeeId)
                : FilterDefinition<Attendance>.Empty;
            
            var totalFilter = queryGtDate & queryLtDate & queryEmployeeId;
            var result = await _attendanceRepo.QueryAttendance(totalFilter, pagingParams);
            return new OkResponse(result);
        }

        [HttpPost]
        [RoleWithSuperAdmin(Constants.Roles.Hr)]
        public async Task<JsonResult> PostListAttendance([FromBody] List<Attendance> listAttendance)
        {
            if (listAttendance == null)
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Danh sách không đúng định dạng!"
                });
            }

            try
            {
                await _attendanceRepo.InsertManyAttendance(listAttendance);
                return new OkResponse(new
                {
                    Message = "Thêm danh sách điểm danh thành công!",
                    Data = listAttendance
                });
            }
            catch (Exception e)
            {
                return ExceptionHelper.HandleError(e);
            }
        }

        [Route("excels")]
        [HttpPost]
        [RoleWithSuperAdmin(Constants.Roles.Hr)]
        public async Task<JsonResult> PostExcelListAttendance(IFormFile file)
        {
            if (file == null)
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "File không đúng!"
                });
            }

            var steam = new MemoryStream();
            await file.CopyToAsync(steam);
            using var package = new ExcelPackage(steam);
            var listAttendance = ExcelHelpers.ReadExcelAttendance(package);

            try
            {
                await _attendanceRepo.InsertManyAttendance(listAttendance);
                return new OkResponse(new
                {
                    Message = "Thêm danh sách điểm danh thành công!",
                    Data = listAttendance
                });
            }
            catch (Exception e)
            {
                return ExceptionHelper.HandleError(e);
            }
        }

        [Route("excels")]
        [HttpGet]
        [AllowAllSystemUser]
        public async Task<JsonResult> ReadExcel(IFormFile file)
        {
            if (file == null)
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "File không đúng!"
                });
            }

            var steam = new MemoryStream();
            await file.CopyToAsync(steam);
            using var package = new ExcelPackage(steam);
            var listAttendance = ExcelHelpers.ReadExcelAttendance(package); 
            var result = await CheckAvailableInsertListAttendance(listAttendance);

            return new OkResponse(result);
        }

        [Route("{attendanceId}")]
        [HttpGet]
        [AllowAllSystemUser]
        public async Task<JsonResult> GetAttendance(string attendanceId)
        {
            return new OkResponse(await _attendanceRepo.FindFirstById(attendanceId));
        }

        [Route("{attendanceId}")]
        [HttpPut]
        [AllowAllSystemUser]
        public async Task<JsonResult> ModifyAttendance(string attendanceId, [FromBody] Attendance attendance)
        {
            var result = await _attendanceRepo.ReplaceOneById(attendanceId, attendance);
            if (result.ModifiedCount.Equals(1))
            {
                return new OkResponse(new
                {
                    Message = "Thay đổi thành công!"
                });
            }
            else
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Lỗi khi thay đổi dữ liệu!"
                });
            }
        }

        [Route("{attendanceId}")]
        [HttpDelete]
        [RoleWithSuperAdmin(Constants.Roles.Hr)]
        public async Task<JsonResult> DeleteAttendance(string attendanceId)
        {
            var result = await _attendanceRepo.DeleteOneById(attendanceId);
            if (result)
            {
                return new OkResponse(new
                {
                    Message = "Xóa thành công!"
                });
            }
            else
            {
                return new BadRequestResponse(new ErrorData
                {
                    Message = "Lỗi khi xóa!"
                });
            }
        }

        private async Task<List<Attendance>> CheckAvailableInsertListAttendance(List<Attendance> listInput)
        {
            var listResult = new List<Attendance>();
            
            var listUnique = listInput.Select(x => x.Unique).ToList();
            var filter = Builders<Attendance>.Filter.Where(x => listUnique.Contains(x.Unique));
            var result = await _attendanceRepo.QueryAttendance(filter);
            listInput.ForEach(attendance =>
            {
                var existAttendance = result.Find(x => x.Unique.Equals(attendance.Unique));
                if (existAttendance != null)
                {
                    attendance.Id = existAttendance.Id;
                    attendance.InsertAvailable = false;
                }
                else
                {
                    attendance.InsertAvailable = true;
                }
                listResult.Add(attendance);
            });
            return listResult;
        }
    }
}