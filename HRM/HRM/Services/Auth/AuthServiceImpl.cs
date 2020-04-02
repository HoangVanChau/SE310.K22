using System;
using System.Collections.Generic;
using System.Text;
using HRM.Helpers;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace HRM.Services.Auth
{
    public class AuthServiceImpl : IAuthService
    {
        private readonly IAuthSetting _authSetting;
        
        public AuthServiceImpl(IAuthSetting authSetting)
        {
            _authSetting = authSetting;
        }

        public string GenerateAccessToken(string userId)
        {
            
            var token = new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(_authSetting.SecretCode)
                .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(_authSetting.AccessTokenExpire).ToUnixTimeSeconds())
                .AddClaim("userId", userId)
                .Build();

            return token;
        }
        
        public string GenerateRefreshToken(string userId)
        {
            var token = new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(_authSetting.SecretCode)
                .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(_authSetting.RefreshTokenExpire).ToUnixTimeSeconds())
                .AddClaim("userId", userId)
                .Build();

            return token;
        }
        public string VerifyToken(string token)
        {
            try
            {
                var payload = new JwtBuilder()
                    .WithSecret(_authSetting.SecretCode)
                    .MustVerifySignature()
                    .Decode<IDictionary<string, object>>(token);

                return payload["userId"].ToString();
            }
            catch (TokenExpiredException)
            {
                return null;
            }
            catch (SignatureVerificationException)
            {
                return null;
            }
        }
        public string HashPassword(string password)
        {
            byte[] salt = Encoding.ASCII.GetBytes(_authSetting.SecretCode);
            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt, 
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8
                )
            );
            return hash;
        }
    }
}