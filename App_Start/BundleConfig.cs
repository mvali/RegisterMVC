using System.Web;
using System.Web.Optimization;

namespace Register
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Assets/Scripts/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Assets/Scripts/jquery.validate.min.js",
                        "~/Assets/Scripts/jquery.validate.unobtrusive.js",
                        "~/Assets/Scripts/jquery.unobtrusive-ajax.min.js",
                        "~/Assets/Scripts/expressive.annotations.validate.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Assets/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Assets/Scripts/bootstrap.js",
                      "~/Assets/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/content/css").Include("~/Assets/Css/zeroo.css"));
            /*bundles.Add(new StyleBundle("~/content/css").Include(
                      "~/Assets/Css/bootstrap.min.css",
                      "~/Assets/Css/style.css",
                      "~/Assets/Css/responsive.css"));*/
        }
    }
}
