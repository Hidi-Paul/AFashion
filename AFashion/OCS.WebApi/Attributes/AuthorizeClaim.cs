using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace OCS.WebApi.Attributes
{
    public class AuthorizeClaim : AuthorizeAttribute
    {
        private string claimType;
        private string claimValue;

        public AuthorizeClaim(string type, string value)
        {
            claimType = type;
            claimValue = value;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var owinContext = HttpContext.Current.GetOwinContext();
            ClaimsPrincipal user = owinContext.Authentication.User;

            IEnumerable<Claim> claims = user.Claims;
            foreach (Claim claim in claims)
            {
                if (claim.Type.Equals(claimType) &&
                    claim.Value.Equals(claimValue))
                {
                    //Has Claim
                    base.OnAuthorization(actionContext);
                    return;
                }
            }
            //Had Not Claim, Not has claim, has not claim
            base.HandleUnauthorizedRequest(actionContext);
        }
    }
}