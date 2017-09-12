using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OCS.WebApi.Security
{
    public class AuthorizationProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            /*
            string clientId = string.Empty;
            string clientSecret = string.Empty;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.SetError("invalid_client", "Invalid Authorization header, creditentials could not be retrieved");
                context.Rejected();
            }
            else
            {
                var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

                if (userManager == null)
                {
                    context.SetError("server_error");
                    context.Rejected();
                    return;
                }

                ApplicationUser user = await userManager.FindByIdAsync(Guid.Parse(clientId));

                if (user != null && userManager.PasswordHasher.VerifyHashedPassword(user.PasswordHash, clientSecret) == Microsoft.AspNet.Identity.PasswordVerificationResult.Success)
                {
                    context.OwinContext.Set<ApplicationUser>("auth:user", user);
                    context.Validated(clientId);
                }
                else
                {
                    context.SetError("invalid_client", "Invalid client credentials");
                    context.Rejected();
                }
            }
            */
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
            var user = await userManager.FindAsync(context.UserName, context.Password);
            
            if (user == null)
            {
                context.SetError("invalid_grant", "Username or password incorrect");
                return;
            }
            var identity = await userManager.CreateIdentityAsync(user, context.Options.AuthenticationType);
            AuthenticationProperties props = new AuthenticationProperties(new Dictionary<string, string>()
            {
                { "Sid", user.Id.ToString() },
                { "Name", user.UserName }
            });
            
            var claims = userManager.GetClaimsAsync(user.Id);
            foreach (Claim claim in claims.Result)
            {
                identity.AddClaim(claim);
            }

            var ticket = new AuthenticationTicket(identity,props);

            context.Validated(ticket);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {

            return Task.FromResult<object>(null);
        }
    }
}