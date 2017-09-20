using System.Web.Optimization;

namespace OCS.MVC.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //
            // Scripts
            //

            bundles.Add(new ScriptBundle("~/bundles/external").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.cookie.js",
                "~/Scripts/jquery.validate*",
                "~/Scripts/JQuery-UI/jquery-ui.js",
                "~/Scripts/modernizr-*",
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                "~/Scripts/Site/HttpRequestHelper.js"
                ));

            //
            //  StyleSheets
            //

            bundles.Add(new StyleBundle("~/Content/external").Include(
                "~/Content/bootstrap.css",
                "~/Scripts/JQuery-UI/jquery-ui.css",
                "~/Scripts/JQuery-UI/jquery-ui.structure.css",
                "~/Scripts/JQuery-UI/jquery-ui.theme.css"
                ));

            bundles.Add(new StyleBundle("~/Content/custom").Include(
                "~/Content/Site.css",
                "~/Content/Account.css"
                ));

            BundleTable.EnableOptimizations = true;
        }
    }
}