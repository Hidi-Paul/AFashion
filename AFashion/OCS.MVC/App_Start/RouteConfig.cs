using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OCS.MVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
               name: "TenantRoute",
               url: "g-{tenant}/{controller}/{action}/{id}",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });

            routes.MapRoute(
                 name: "GuestRoute",
                 url: "{controller}/{action}/{id}",
                 defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });

            routes.MapRoute(
                 name: "Default",
                 url: "g-{tenant}/{controller}/{action}/{id}",
                 defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional, tenant="" });
        }
    }
}
