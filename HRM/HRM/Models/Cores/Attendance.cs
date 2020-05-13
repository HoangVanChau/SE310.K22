using System;
using System.Collections.Generic;
using HRM.Models.Bases;
using MongoDB.Bson.Serialization.Attributes;

namespace HRM.Models.Cores
{
    public class Attendance : BaseModel
    {
        [BsonDateTimeOptions(DateOnly = true)]
        public DateTime Date { get; set; }
        public string EmployeeId { get; set; }
        public List<InOutTime> Data { get; set; }
        public int SummaryWorkingTimeInMinute { get; set; }
        public int LateTime { get; set; }
    }
}