using System.Web.Optimization;

namespace ExpressQuiz
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/expressquiz").Include(
                "~/Scripts/expressquiz/utils.js",
                "~/Scripts/expressquiz/answer.js",
                "~/Scripts/expressquiz/question.js",
                "~/Scripts/expressquiz/quiz.js",
                "~/Scripts/expressquiz/runtime.js",
                "~/Scripts/expressquiz/useranswer.js",
                "~/Scripts/expressquiz/countdown.js",
                "~/Scripts/expressquiz/ajaxhelper.js",
                "~/Scripts/expressquiz/activequiz.js",
                "~/Scripts/expressquiz/ui.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css"));

#if DEBUG
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}