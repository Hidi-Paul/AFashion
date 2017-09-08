using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using OCS.WebApi.App_Start;
using OCS.WebApi.Models;
using OCS.WebApi.Security;
using Owin;
using System;

namespace OCS.WebApi
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext(SecurityDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                AuthorizeEndpointPath = new PathString("/Account/Login"),
                Provider = new AuthorizationProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                AllowInsecureHttp = true
            };

            app.UseOAuthBearerTokens(OAuthOptions);
            app.UseOAuthBearerAuthentication(
                new OAuthBearerAuthenticationOptions
                {
                    Provider = new OAuthBearerAuthenticationProvider()
                }
            );
        }

    }
}