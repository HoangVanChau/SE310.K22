using System;
using HRM.Models.Cores;

namespace HRM.Models.Requests
{
    public class UserRegisterRequest
    {
        public String UserName { get; set; }
        public String FullName { get; set; }
        public String Password { get; set; }
        public String PhoneNumber { get; set; }
        public String Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Address Address { get; set; }
        
        public String AvatarImageId { get; set; }
    }
}