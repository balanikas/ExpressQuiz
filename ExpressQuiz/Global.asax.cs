﻿using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;


namespace ExpressQuiz
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DependencyRegistrar.Register();
        }

    

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            if (exception == null)
            {
                return;
            }
                
            Response.Clear();
            Response.Redirect(String.Format("~/Home/Error/?message={0}", exception.Message));

        }
    }
}
