using System.Web.Mvc;

namespace OCS.MVC.Controllers
{
    public abstract class BaseController : Controller
    {
        protected string GetUniqueKey()
        {
            return RouteData.Values["guid"].ToString();
        }

        private void SetUniqueSession(string name, object value)
        {
            Session[GetUniqueKey() + "_" + name] = value;
        }
        public object GetSession(string name)
        {
            return Session[GetUniqueKey() + "_" + name];
        }
    }
}