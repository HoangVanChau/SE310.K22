using System;

namespace HRM.Models.Responses
{
    public class AuthResponse
    {
        public String AccessToken { get; set; }
        public String RefreshToken { get; set; }
    }
}