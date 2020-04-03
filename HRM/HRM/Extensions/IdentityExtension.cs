using System.Security.Claims;
using System.Security.Principal;

namespace HRM.Extensions
{
    public static class IdentityExtension
    {
        public static string GetId(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;

            Claim claim = claimsIdentity?.FindFirst(ClaimTypes.Name);

            return claim?.Value;
        }
    }
}