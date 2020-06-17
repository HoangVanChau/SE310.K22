using System;
using System.Collections.Generic;
using HRM.Models.Bases;
using MongoDB.Bson.Serialization.Attributes;

namespace HRM.Models.Cores
{
    [BsonIgnoreExtraElements]
    public class User : BaseModel
    {
        public String UserId { get; set; } //guid auto gen
        public uint EmployeeId { get; set; } //auto increase
        public String FullName { get; set; }
        public String UserName { get; set; }
        public String Email { get; set; }
        
        public String AvatarImageId { get; set; }
        public String PhoneNumber { get; set; }
        public Address Address { get; set; }
        
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime DateOfBirth { get; set; }
        public String Role { get; set; }
        public double YearRemainDayOffs { get; set; }
        
        //temp field
        public List<Team> Teams { get; set; }
    }
}