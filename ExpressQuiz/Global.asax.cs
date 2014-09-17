using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ExpressQuiz.Controllers;
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
            Exception lastError = Server.GetLastError();
            Server.ClearError();

            int statusCode = 0;

            if (lastError.GetType() == typeof(HttpException))
            {
                statusCode = ((HttpException)lastError).GetHttpCode();
            }
            else
            {
                statusCode = 500;
            }

            var contextWrapper = new HttpContextWrapper(this.Context);

            var routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            routeData.Values.Add("action", "Index");
            routeData.Values.Add("statusCode", statusCode);
            routeData.Values.Add("exception", lastError);
            routeData.Values.Add("isAjaxRequet", contextWrapper.Request.IsAjaxRequest());

            IController controller = new ErrorController();

            var requestContext = new RequestContext(contextWrapper, routeData);

            controller.Execute(requestContext);
            Response.End();
        }
    }
}