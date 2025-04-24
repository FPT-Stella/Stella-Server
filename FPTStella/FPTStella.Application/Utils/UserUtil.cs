using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System.Security.Claims;

namespace FPTStella.Application.Utils
{
    public class UserUtil
    {
        public static Guid? GetAccountId(HttpContext httpContext)
        {
            if (httpContext == null || httpContext.User == null)
            {
                return null;
            }

            var nameIdentifierClaim = httpContext.User.FindFirst("id");

            if (nameIdentifierClaim == null)
            {
                return null;
            }

            if (!Guid.TryParse(nameIdentifierClaim.Value, out Guid accountId))
            {
                throw new ArgumentException("Invalid account id format", nameIdentifierClaim.Value);
            }
            return accountId;
        }

        public static string GetName(HttpContext httpContext)
        {
            var nameClaim = httpContext.User.FindFirst("username");
            return nameClaim?.Value;
        }

        public static string GetRoleName(HttpContext httpContext)
        {
            var roleClaim = httpContext.User.FindFirst(ClaimTypes.Role);
            return roleClaim?.Value;
        }
    }
}
