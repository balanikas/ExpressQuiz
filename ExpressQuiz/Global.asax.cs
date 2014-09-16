using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ExpressQuiz.Core.Utils;
using StackExchange.Profiling;
using StackExchange.Profiling.EntityFramework6;

namespace ExpressQuiz
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
#if DEBUG
            MiniProfilerEF6.Initialize();
#endif

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DependencyRegistrar.Register();
        }


        protected void Application_BeginRequest()
        {
#if DEBUG
            if (Request.IsLocal)
            {
                MiniProfiler.Start();
            }
#endif
        }


        protected void Application_EndRequest()
        {
#if DEBUG
            if (Request.IsLocal)
            {
                MiniProfiler.Stop();
            }
#endif
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            if (exception == null)
            {
                return;
            }

            new Logger().Error("Application error! " + exception.Message);

            Response.Clear();
            Response.Redirect(String.Format("~/Home/Error/?message={0}", exception.Message));
        }
    }
}