using System;

namespace HRM.Models.QueryParams
{
    public class AttendanceQuery
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public uint? EmployeeId { get; set; }
    }
}