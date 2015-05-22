using System.Web;
using System.Web.Optimization;

namespace QueueStepchenko
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/JS").Include(
                               "~/Scripts/jquery-{version}.js",
                               "~/Scripts/jquery.validate*",
                               "~/Scripts/modernizr-*",
                               "~/Scripts/bootstrap.js",
                               "~/Scripts/respond.js",
                               "~/Scripts/jquery-1.10.2.min.js",
                               "~/Scripts/jquery.unobtrusive-ajax.js",
                               "~/Scripts/jquery.signalR-2.2.0.min.js",
                               "~/Scripts/util.js"
                               ));

           
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
