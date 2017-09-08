using Microsoft.Owin;
using OCS.WebApi.App_Start;
using Owin;
using System.Web.Http;

[assembly: OwinStartup(typeof(OCS.WebApi.Startup))]
namespace OCS.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            UnityWebApiActivator.Start();
            BusinessLayer.Config.AutoMapperServicesConfig.Configure();
            ConfigureAuth(app);
        }
    }
}
