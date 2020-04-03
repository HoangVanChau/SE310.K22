using System;
using System.Collections.Generic;
using HRM.Models.Base;

namespace HRM.Models
{
    public class VerifyUserResponse
    {
        public String UserId { get; set; } //guid auto gen
        public int EmployeeId { get; set; } //auto increase
        public String FullName { get; set; }
        public String UserName { get; set; }
        public String Email { get; set; }
        public String PhoneNumber { get; set; }
        public Address Address { get; set; }
        public String Role { get; set; }
    }
}