using System.Web.Http;

namespace OCS.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            BusinessLayer.Config.AutoMapperServicesConfig.Configure();
        }
    }
}
