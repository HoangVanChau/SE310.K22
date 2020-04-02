using System;
using HRM.Models.Cores;

namespace HRM.Models
{
    public class AuthResponse
    {
        public String AccessToken { get; set; }
        public String RefreshToken { get; set; }
    }
}