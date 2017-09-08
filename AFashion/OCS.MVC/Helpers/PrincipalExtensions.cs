using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace OCS.MVC.Helpers
{
    public static class PrincipalExtensions
    {
        public static bool ClaimExists(this IPrincipal principal, string claimType)
        {
            var claimsPrincipal = principal as ClaimsPrincipal;
            if (claimsPrincipal == null)
            {
                return false;
            }

            var claim = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == claimType);

            return claim != null;
        }

        public static bool HasClaim(this IPrincipal principal, string claimType, string claimValue)
        {
            var claimsPrincipal = principal as ClaimsPrincipal;
            if (claimsPrincipal == null)
            {
                return false;
            }

            var claim = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == claimType && x.Value == claimValue);

            return claim != null;
        }

        public static string GetClaim(this IPrincipal principal, string claimType)
        {
            var claimsPrincipal = principal as ClaimsPrincipal;
            if (claimsPrincipal == null)
            {
                return string.Empty;
            }

            var claim = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == claimType);
            if (claim == null)
            {
                return string.Empty;
            }

            return claim.Value;
        }
    }
}