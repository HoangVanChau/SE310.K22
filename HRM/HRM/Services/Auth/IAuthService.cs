using System;
using HRM.Models;
using HRM.Models.Cores;

namespace HRM.Services.Auth
{
    public interface IAuthService
    {
        public String GenerateAccessToken(String userId);
        public String GenerateRefreshToken(String userId);
        public String VerifyToken(String token);
        public string HashPassword(string password);
    }
}