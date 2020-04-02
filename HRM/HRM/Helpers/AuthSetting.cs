using System;
using System.Security.Cryptography.X509Certificates;
using HRM.Services.Auth;

namespace HRM.Helpers
{
    public class AuthSetting : IAuthSetting
    {
        public String SecretCode { get; set; }
        public String HashCode { get; set; }
        public int AccessTokenExpire { get; set; }
        public int RefreshTokenExpire { get; set; }
    }
    
    public interface IAuthSetting
    {
        public String SecretCode { get; set; }
        public String HashCode { get; set; }
        public int AccessTokenExpire { get; set; }
        public int RefreshTokenExpire { get; set; }
    }
}