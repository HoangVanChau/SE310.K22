using HRM.Models.Bases;
using MongoDB.Bson.Serialization.Attributes;

namespace HRM.Models.Cores
{
    [BsonIgnoreExtraElements]
    public class Payroll : BaseModel
    {
        public bool IsApprove { get; set; } = false;
        public string UserId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int BaseSalary { get; set; }
        public MonthlyBonus Bonus { get; set; }
        public int SocialInsurance { get; set; }
        public int TotalSalary { get; set; }
        public int PersonalIncomeTax { get; set; }
        public int TotalExpectWorkingHours { get; set; }
        public double TotalWorkingHours { get; set; }
        public double UnPaidLeaveHours { get; set; }
        public double PaidLeaveHours { get; set; }
        public double PaidLeaveTotal { get; set; }
        
        public double FinalReceiveSalary { get; set; }
    }
}