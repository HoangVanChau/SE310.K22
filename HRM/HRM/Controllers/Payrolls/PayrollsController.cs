using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HRM.Constants;
using HRM.Helpers;
using HRM.Models.Cores;
using HRM.Models.Responses.Bases;
using HRM.Repositories;
using HRM.Repositories.Attendance;
using HRM.Repositories.Contract;
using HRM.Repositories.DateOff;
using HRM.Repositories.Payroll;
using HRM.Repositories.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace HRM.Controllers.Payrolls
{
    [Route("api/[controller]")]
    public class PayrollsController : ControllerBase
    {
        private readonly IContractRepository _contractRepo;
        private readonly IDateOffRepository _dateOffRepo;
        private readonly IAttendanceRepository _attendanceRepo;
        private readonly IUserRepository _userRepo;
        private readonly IPayrollRepository _payrollRepo;
        private readonly IHolidayRepository _holidayRepo;
        public PayrollsController(IUserRepository userRepo, IContractRepository contractRepo, IDateOffRepository dateOffRepo, IAttendanceRepository attendanceRepo, IPayrollRepository payrollRepo, IHolidayRepository holidayRepo)
        {
            _userRepo = userRepo;
            _attendanceRepo = attendanceRepo;
            _contractRepo = contractRepo;
            _dateOffRepo = dateOffRepo;
            _payrollRepo = payrollRepo;
            _holidayRepo = holidayRepo;
        }
        
        [HttpGet]
        [RoleWithSuperAdmin(Constants.Roles.Hr)]
        public async Task<JsonResult> GetAll()
        {
            var filter = Builders<Payroll>.Filter.Empty;
            var result = await _payrollRepo.QueryMany(filter);
            return new OkResponse(result);
        }
        
        [HttpGet]
        [RoleWithSuperAdmin(Constants.Roles.Hr)]
        [Route("{userId}")]
        public async Task<JsonResult> GetByUserId(string userId)
        {
            var filter = Builders<Payroll>.Filter.Eq(x => x.UserId, userId);
            var result = await _payrollRepo.QueryMany(filter);
            return new OkResponse(result);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("calculate/{userId}")]
        public async Task<JsonResult> Test(string userId, [FromQuery] int month, [FromQuery] int year)
        {
            var user = await _userRepo.FindUserByUserId(userId);
            var filterContractActive = Builders<Contract>.Filter.Eq(x => x.Active, true);
            var filterContractUserId = Builders<Contract>.Filter.Eq(x => x.UserId, user.UserId);
            
            var contract = await 
                _contractRepo.QueryContract(filterContractActive & filterContractUserId);

            var salary = await CalculateContractSalary(contract, month, year);
            await _payrollRepo.InsertOne(salary);
            return new OkResponse(salary);
        }
        
        public async Task<Payroll> CalculateContractSalary(Contract contract, int month, int year)
        {
            var user = await _userRepo.FindUserByUserId(contract.UserId);
            var firstDate = new DateTime(year, month, 1);
            var endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            var filterAttendanceDateGt = Builders<Attendance>.Filter.Gte(x => x.Unique.Date, firstDate);
            var filterAttendanceDateLt = Builders<Attendance>.Filter.Lte(x => x.Unique.Date, endDate);
            var filterAttendanceUserId = Builders<Attendance>.Filter.Eq(x => x.Unique.EmployeeId, user.EmployeeId);
            var attendances = await _attendanceRepo.QueryAttendance(
                customFilter: filterAttendanceDateGt & filterAttendanceDateLt & filterAttendanceUserId);
            
            var filterDateOffGt = Builders<DateOff>.Filter.Gte(x => x.Date, firstDate);
            var filterDateOffLt = Builders<DateOff>.Filter.Lte(x => x.Date, endDate);
            var filterDateOffUserId = Builders<DateOff>.Filter.Eq(x => x.UserId, user.UserId);
            var dateOffs = await 
                _dateOffRepo.QueryDateOffs(filterDateOffGt & filterDateOffLt & filterDateOffUserId);
            
            var filterHolidayGte = Builders<Holiday>.Filter.Gte(x => x.Date, firstDate);
            var filterHolidayLte = Builders<Holiday>.Filter.Lte(x => x.Date, endDate);
            var holidays = await _holidayRepo.QueryMany(filterHolidayGte & filterHolidayLte);

            var payroll = SalaryHelper.CalculateSalary(month, year,holidays, attendances, dateOffs, contract);
            return payroll;
        }
    }
}