using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace HRM.Constants
{
    public static class Roles
    {
        public const String Member = "Member";
        public const String Employee = "Employee";
        public const String Manager = "Manager";
        public const String Director = "Director";
        public const String Hr = "Hr";
        public const String SuperAdmin = "SuperAdmin";

        public static List<string> AllRoles()
        {
            return new List<string>
            {
                Member, Employee, Manager, Director, Hr, SuperAdmin
            };
        }
    }

    public class AllowAllSystemUser : AuthorizeAttribute
    {
        public AllowAllSystemUser()
        {
            Roles = GetAllRole();
        }

        private string GetAllRole()
        {
            return Constants.Roles.SuperAdmin + "," +
                   Constants.Roles.Member + "," +
                   Constants.Roles.Employee + "," +
                   Constants.Roles.Manager + "," +
                   Constants.Roles.Director + "," +
                   Constants.Roles.Hr;
        }
    }
    
    public class AllowChangeRole : AuthorizeAttribute
    {
        public AllowChangeRole()
        {
            Roles = Constants.Roles.SuperAdmin + "," + Constants.Roles.Hr;
        }
    }
}