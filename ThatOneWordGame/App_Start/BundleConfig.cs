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

			var tsBundle = new ScriptBundle("~/bundles/ts");
			tsBundle.Include(
						"~/Scripts/ts/Component.js",
						"~/Scripts/ts/Animator.js",
						"~/Scripts/ts/Hub.js",
						"~/Scripts/ts/Models/Player.js",
						"~/Scripts/ts/Models/DataModel.js",
						"~/Scripts/ts/Application.js",
						"~/Scripts/ts/Components/Connecting.js",
						"~/Scripts/ts/Components/Hello.js",
						"~/Scripts/ts/Components/PlayUI.js");
#if DEBUG
			tsBundle.Include("~/Scripts/ts/Debug.js");
#endif
			bundles.Add(tsBundle);

			bundles.Add(new StyleBundle("~/Content/css").Include(
					  "~/Content/css/Site.css", new CssRewriteUrlTransform()));

			// Set EnableOptimizations to false for debugging. For more information,
			// visit http://go.microsoft.com/fwlink/?LinkId=301862
#if DEBUG
			BundleTable.EnableOptimizations = false;
#else
			BundleTable.EnableOptimizations = true;
#endif
		}
	}
}
