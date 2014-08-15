﻿using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using ExpressQuiz.Models;
using ExpressQuiz.Repos;

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
            Register();
        }

        private void Register()
        {
            var cb = new Autofac.ContainerBuilder();

            cb.RegisterType<QuizDbContext>().InstancePerRequest();
            cb.RegisterType<Repo<Quiz>>().As<IRepo<Quiz>>().InstancePerRequest();
            cb.RegisterType<Repo<Question>>().As<IRepo<Question>>().InstancePerRequest();
            cb.RegisterType<Repo<Answer>>().As<IRepo<Answer>>().InstancePerRequest();
            cb.RegisterType<Repo<QuizCategory>>().As<IRepo<QuizCategory>>().InstancePerRequest();
            cb.RegisterType<Repo<QuizRating>>().As<IRepo<QuizRating>>().InstancePerRequest();
            cb.RegisterType<Repo<QuizResult>>().As<IRepo<QuizResult>>().InstancePerRequest();

            cb.RegisterControllers(typeof(MvcApplication).Assembly).InstancePerRequest();

            cb.RegisterAssemblyModules(typeof(MvcApplication).Assembly);
           
            var container = cb.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Response.Clear();
            Response.Redirect(String.Format("~/Views/Shared/Error/?message={0}", exception.Message));

        }
    }
}
