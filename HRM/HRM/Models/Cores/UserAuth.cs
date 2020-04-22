using System;

namespace HRM.Models.Cores
{
    public class UserAuth : User
    {
        public String HashPassword { get; set; }
    }
}