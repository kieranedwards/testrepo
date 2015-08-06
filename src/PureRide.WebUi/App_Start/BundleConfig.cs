using System.Web.Optimization;
using System.Web.Optimization.React;

namespace PureRide.WebUI
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/core").Include("~/Scripts/Javascript/reqwest.min.js", "~/Scripts/Javascript/bling.js", "~/Scripts/Javascript/core.js"));
            bundles.Add(new JsxBundle("~/bundles/locations").Include("~/Scripts/JSX/locations.react.jsx"));
            bundles.Add(new JsxBundle("~/bundles/credits").Include("~/Scripts/JSX/creditPacks.react.jsx"));
            bundles.Add(new JsxBundle("~/bundles/booking").Include("~/Scripts/JSX/dialog.react.jsx","~/Scripts/JSX/classSchedule.react.jsx", "~/Scripts/JSX/classSeatSelection.react.jsx"));
            bundles.Add(new JsxBundle("~/bundles/account").Include("~/Scripts/JSX/accountDashboard.react.jsx"));
          
            //Comment this out to control this setting via web.config compilation debug attribute
            BundleTable.EnableOptimizations = false;
        }
    }
}