using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using OCS.MVC.Security;
using Owin;
using System.Configuration;
using System.Security.Claims;
using System.Web.Helpers;

namespace OCS.MVC
{
    public partial class Startup
    {

        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                CookieName= ConfigurationManager.AppSettings["Auth-CookieName"],
                CookieManager = new MultiTennantCookieManager(),
                LoginPath = new PathString("/Account/Login")
            });

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Name;
        }
    }
}