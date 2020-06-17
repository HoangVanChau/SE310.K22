using System;
using HRM.Models.Cores;

namespace HRM.Models.Requests
{
    public class UserModifyRequest
    {
        public String UserName { get; set; }
        public String Email { get; set; }
        public String PhoneNumber { get; set; }
        public String DateOfBirth { get; set; }
        public String FullName { get; set; }
        public Address Address { get; set; }
        public String AvatarImageId { get; set; }
        public Double? YearRemainDayOffs { get; set; }
    }
}