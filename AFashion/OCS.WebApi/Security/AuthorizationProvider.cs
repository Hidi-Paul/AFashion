using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;
using Microsoft.AspNet.Identity.Owin;
using OCS.WebApi.SecurityModels;
using System;
using System.Security.Claims;
using System.Collections.Generic;

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

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            identity.AddClaim(new Claim("Sid", user.Id.ToString()));
            identity.AddClaim(new Claim("Name", context.UserName));

            var claims = userManager.GetClaimsAsync(user.Id);
            foreach (Claim claim in claims.Result)
            {
                identity.AddClaim(claim);
            }

            context.Validated(identity);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            var claims = userManager.GetClaimsAsync(Guid.Parse(context.Identity.FindFirst(x=>x.Type.Equals("Sid")).Value));
            foreach (Claim claim in claims.Result)
            {
                context.AdditionalResponseParameters.Add(claim.Type, claim.Value);
            }
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
    }
}