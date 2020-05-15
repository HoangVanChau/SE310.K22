using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using HRM.Extensions.JsonConverters;
using HRM.Models.Bases;
using MongoDB.Bson.Serialization.Attributes;

namespace HRM.Models.Cores
{
    public class Attendance : BaseModel
    {
        public List<InOutTime> Data { get; set; }
        public AttendanceUnique Unique { get; set; }
        [BsonIgnore] public bool InsertAvailable { get; set; }
    }

    public class AttendanceUnique : IEquatable<Attendance>
    {
        [BsonDateTimeOptions(DateOnly = true)]
        public DateTime Date { get; set; }
        public uint EmployeeId { get; set; }

        protected bool Equals(AttendanceUnique other)
        {
            return Date.Equals(other.Date) && EmployeeId == other.EmployeeId;
        }

        public bool Equals(Attendance other)
        {
            return other != null && Equals(other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AttendanceUnique) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Date, EmployeeId);
        }
    }
    
    public class InOutTime
    {
        public TimeSpan In { get; set; }
        public TimeSpan Out { get; set; }
    }
}