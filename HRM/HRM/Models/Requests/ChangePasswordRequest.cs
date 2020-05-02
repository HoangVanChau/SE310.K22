using System;

namespace HRM.Models.Requests
{
    public class ChangePasswordRequest
    {
        public String OldPassword { get; set; }
        public String NewPassword { get; set; }
    }
}