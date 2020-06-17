using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HRM.Helpers;
using HRM.Models.Cores;
using HRM.Repositories;
using HRM.Repositories.Attendance;
using HRM.Repositories.Contract;
using HRM.Repositories.DateOff;
using HRM.Repositories.Payroll;
using HRM.Repositories.User;
using MongoDB.Driver;

namespace HRM.Schedulers
{
    public class CalculatePayrollTask 
    {
        private readonly IContractRepository _contractRepo;
        private readonly IDateOffRepository _dateOffRepo;
        private readonly IAttendanceRepository _attendanceRepo;
        private readonly IUserRepository _userRepo;
        private readonly IPayrollRepository _payrollRepo;
        private readonly IHolidayRepository _holidayRepo;
        public CalculatePayrollTask(IUserRepository userRepo, IContractRepository contractRepo, IDateOffRepository dateOffRepo, IAttendanceRepository attendanceRepo, IPayrollRepository payrollRepo, IHolidayRepository holidayRepo)
        {
            _userRepo = userRepo;
            _attendanceRepo = attendanceRepo;
            _contractRepo = contractRepo;
            _dateOffRepo = dateOffRepo;
            _payrollRepo = payrollRepo;
            _holidayRepo = holidayRepo;
        }
        public async Task Invoke()
        {
            var lastMonthDate = DateTime.Now.AddMonths(-1);
            var firstDate = new DateTime(lastMonthDate.Year, lastMonthDate.Month, 1);
            var endDate = new DateTime(lastMonthDate.Year, lastMonthDate.Month, DateTime.DaysInMonth(lastMonthDate.Year, lastMonthDate.Month));
            
            var filterContract = Builders<Contract>.Filter.Eq(x => x.Active, true);
            var listActiveContract = await _contractRepo.QueryContracts(filterContract);
            
            var filterHolidayGte = Builders<Holiday>.Filter.Gte(x => x.Date, firstDate);
            var filterHolidayLte = Builders<Holiday>.Filter.Lte(x => x.Date, endDate);
            var holidays = await _holidayRepo.QueryMany(filterHolidayGte & filterHolidayLte);

            var listPayrolls = new List<Payroll>();
            foreach (var contract in listActiveContract)
            {    
                listPayrolls.Add(await CalculateContractSalary(contract, firstDate, endDate, holidays));
            }
            
            await _payrollRepo.InsertMany(listPayrolls);
        }
        
        public async Task<Payroll> CalculateContractSalary(Contract contract, DateTime fromDate, DateTime toDate, List<Holiday> holidays)
        {
            var user = await _userRepo.FindUserByUserId(contract.UserId);

            var filterAttendanceDateGt = Builders<Attendance>.Filter.Gte(x => x.Unique.Date, fromDate);
            var filterAttendanceDateLt = Builders<Attendance>.Filter.Lte(x => x.Unique.Date, toDate);
            var filterAttendanceUserId = Builders<Attendance>.Filter.Eq(x => x.Unique.EmployeeId, user.EmployeeId);
            var attendances = await _attendanceRepo.QueryAttendance(
                customFilter: filterAttendanceDateGt & filterAttendanceDateLt & filterAttendanceUserId);
            
            var filterDateOffGt = Builders<DateOff>.Filter.Gte(x => x.Date, fromDate);
            var filterDateOffLt = Builders<DateOff>.Filter.Lte(x => x.Date, toDate);
            var filterDateOffUserId = Builders<DateOff>.Filter.Eq(x => x.UserId, user.UserId);
            var dateOffs = await 
                _dateOffRepo.QueryDateOffs(filterDateOffGt & filterDateOffLt & filterDateOffUserId);

            var payroll = SalaryHelper.CalculateSalary(fromDate.Month, fromDate.Year, holidays, attendances, dateOffs, contract);
            return payroll;
        }
    }
}