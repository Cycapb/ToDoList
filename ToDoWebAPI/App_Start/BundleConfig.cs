using System.Web.Optimization;

namespace ToDoWebAPI
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-theme.css",
                "~/Site.css"));

            bundles.Add(new ScriptBundle("~/Scripts/knockout").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/knockout-{version}.js"));
        }
    }
}