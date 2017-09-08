using System;
using System.Security.Claims;

namespace OCS.WebApi.SecurityModels
{
    public class ApplicationClaimsPrincipal : ClaimsPrincipal
    {
        public ApplicationClaimsPrincipal(ClaimsPrincipal principal) : base(principal)
        {

        }

        public Guid UserId
        {
            get { return Guid.Parse(this.FindFirst(ClaimTypes.Sid).Value); }
        }
    }
}