using System;
using System.Collections.Generic;

namespace HRM.Constants
{
    public static class Role
    {
        public const String Admin = "Admin";
        
        public const String Auth = "Auth";

        public const String OwnInfoRead = "OwnInfoRead";
        public const String OwnInfoModify = "OwnInfoModify";

        public const String AllUserInfoRead = "AllUserInfoRead";
        public const String AllUserInfoWrite = "AllUserInfoWrite";

        public static List<String> AdminRoles()
        {
            return new List<String>
            {
                Auth,
                
                OwnInfoModify,
                OwnInfoRead,
                
                AllUserInfoRead,
                AllUserInfoWrite
            };
        }
        public static List<String> FirstMemberRoles()
        {
            return new List<String>
            {
                Auth,
                
                OwnInfoModify,
                OwnInfoRead
            };
        }
    }
}