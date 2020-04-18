using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace HRM.Constants
{
    public static class Roles
    {
        public const String SuperAdmin = "SuperAdmin";
        public const String Member = "Member";
        public const String Employee = "Employee";
        public const String Manager = "Manager";
        public const String Director = "Director";
    }

    public class AllowAllSystemUser : AuthorizeAttribute
    {
        public AllowAllSystemUser()
        {
            Roles = GetAllRole();
        }

        private string GetAllRole()
        {
            return  Constants.Roles.SuperAdmin + "," +
                    Constants.Roles.Member + "," +
                    Constants.Roles.Employee + "," +
                    Constants.Roles.Manager + "," +
                    Constants.Roles.Director;
        }
    }
}