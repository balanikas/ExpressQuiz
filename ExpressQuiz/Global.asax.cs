using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Xml.Linq;
using ExpressQuiz.Migrations;

namespace ExpressQuiz
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var uri = @"C:\Users\grillo\Documents\GitHub\ExpressQuiz\ExpressQuiz\App_Data\seeddata.xml";
            var quizzes = Configuration.ParseSeedDataFromXml(uri);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
