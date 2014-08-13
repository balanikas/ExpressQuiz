using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.UI;
using System.Xml.Linq;
using Autofac.Core;
using ExpressQuiz.Controllers;
using ExpressQuiz.Migrations;

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
            
            //var ctx = new QuizDbContext();
            //var quizRepo = new Repo<Quiz>(ctx);
            //var questionRepo = new Repo<Question>(ctx);
            //var answerRepo = new Repo<Answer>(ctx);
            //var quizCategoryRepo = new Repo<QuizCategory>(ctx);
            //var quizRatingRepo = new Repo<QuizRating>(ctx);
            //var quizResultRepo = new Repo<QuizResult>(ctx);


            cb.RegisterType<QuizDbContext>().InstancePerRequest();
            cb.RegisterType<Repo<Quiz>>().As<IRepo<Quiz>>().InstancePerRequest();
            cb.RegisterType<Repo<Question>>().As<IRepo<Question>>().InstancePerRequest();
            cb.RegisterType<Repo<Answer>>().As<IRepo<Answer>>().InstancePerRequest();
            cb.RegisterType<Repo<QuizCategory>>().As<IRepo<QuizCategory>>().InstancePerRequest();
            cb.RegisterType<Repo<QuizRating>>().As<IRepo<QuizRating>>().InstancePerRequest();
            cb.RegisterType<Repo<QuizResult>>().As<IRepo<QuizResult>>().InstancePerRequest();

            //cb.RegisterInstance(quizRepo).As<IRepo<Quiz>>();
            //cb.RegisterInstance(questionRepo).As<IRepo<Question>>();
            //cb.RegisterInstance(answerRepo).As<IRepo<Answer>>();
            //cb.RegisterInstance(quizCategoryRepo).As<IRepo<QuizCategory>>();
            //cb.RegisterInstance(quizRatingRepo).As<IRepo<QuizRating>>();
            //cb.RegisterInstance(quizResultRepo).As<IRepo<QuizResult>>();

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
