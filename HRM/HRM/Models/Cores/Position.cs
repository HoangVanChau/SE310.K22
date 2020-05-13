using System;
using HRM.Models.Bases;

namespace HRM.Models.Cores
{
    public class Position : BaseModel
    {
        public string PositionId { get; set; }
        public string PositionName { get; set; }
        public string Description { get; set; }
        public double? BaseMonthSalary { get; set; }
        public double? BaseHourSalary { get; set; }
        public double? BaseOtSalaryPerHour { get; set; }
        public int? BaseDateOff { get; set; }
        
        public double? BaseLateMoney { get; set; }
    }
}