using System;
using System.Collections.Generic;

namespace HRM.Models.Cores
{
    public class TokenPayload
    {
        public String userId { get; set; }
        public String iss { get; set; }
        public long exp { get; set; }

        public Dictionary<String, Object> ToDictionary()
        {
            return new Dictionary<string, object>
            {
                { "userId", userId},
                { "iss", iss},
                { "exp", exp}
            };
        }
    }
}