using System;

namespace HRM.Models.QueryParams
{
    public class UserQuery
    {
        public String? Role { get; set; }
        public Boolean? Available { get; set; }
        public String? Q { get; set; }
        public Boolean? Contractable { get; set; }
    }
}