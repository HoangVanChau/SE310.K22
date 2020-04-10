using System;

namespace HRM.Models.Responses
{
    public class VerifyUserResponse
    {
        public String UserId { get; set; } //guid auto gen
        public uint EmployeeId { get; set; } //auto increase
        public String FullName { get; set; }
        public String UserName { get; set; }
        public String Email { get; set; }
        public String PhoneNumber { get; set; }
        public Address Address { get; set; }
        public String Role { get; set; }
    }
}