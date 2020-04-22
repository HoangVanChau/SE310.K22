using System;
using HRM.Models.Cores;

namespace HRM.Extensions
{
    public static class UserExtensions
    {
        public static User WithoutPassword(this UserAuth user) 
        {
            if (user == null) return null;

            user.HashPassword = null;
            return user;
        }
    }
}