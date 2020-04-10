using System;
using System.Collections.Generic;
using HRM.Models.Bases;

namespace HRM.Models.Cores
{
    public class User : BaseModel
    {
        public String UserId { get; set; } //guid auto gen
        public uint EmployeeId { get; set; } //auto increase
        public String FullName { get; set; }
        public String UserName { get; set; }
        public String Email { get; set; }
        public String PhoneNumber { get; set; }
        public Address Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public String HashPassword { get; set; }
        public String Role { get; set; }
    }
}