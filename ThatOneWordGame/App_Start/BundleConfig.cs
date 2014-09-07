using System.Web;
using System.Web.Optimization;

namespace ThatOneWordGame
{
	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/js").Include(
						"~/Scripts/jquery-1.10.2.min.js",
						"~/Scripts/jquery.signalR-2.1.1.min.js"));

			bundles.Add(new ScriptBundle("~/bundles/ts").Include(
						"~/Scripts/ts/Component.js",
						"~/Scripts/ts/Animator.js",
						"~/Scripts/ts/App.js",
						"~/Scripts/ts/Components/Connecting.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include(
					  "~/Content/css/Site.css", new CssRewriteUrlTransform()));

			// Set EnableOptimizations to false for debugging. For more information,
			// visit http://go.microsoft.com/fwlink/?LinkId=301862
			BundleTable.EnableOptimizations = false;
		}
	}
}
