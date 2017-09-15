using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OCS.MVC
{
    public class UniqueRoute : Route
    {
        private readonly bool isUnique;

        public UniqueRoute(string uri, object defaults) : base(uri, new RouteValueDictionary(defaults), new MvcRouteHandler())
        {
            isUnique = uri.Contains("guid");
            DataTokens = new RouteValueDictionary();
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var routeData = base.GetRouteData(httpContext);
            if (routeData == null)
            {
                return null;
            }
            if (routeData.Values.ContainsKey("guid") && routeData.Values["guid"].ToString().Length == 0)
            {
                return null;
            }
            return routeData;
        }
        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            return !isUnique ? null : base.GetVirtualPath(requestContext, values);
        }
    }

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
