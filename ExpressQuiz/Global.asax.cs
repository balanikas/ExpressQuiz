using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.UI.WebControls;
using Autofac;
using Autofac.Integration.Mvc;
using ExpressQuiz.Controllers;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Repos;
using ExpressQuiz.Core.Services;


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


            cb.RegisterType<QuizService>().As<IQuizService>().InstancePerRequest();
            cb.RegisterType<QuestionService>().As<IQuestionService>().InstancePerRequest();
            cb.RegisterType<AnswerService>().As<IAnswerService>().InstancePerRequest();
            cb.RegisterType<QuizCategoryService>().As<IQuizCategoryService>().InstancePerRequest();


            cb.RegisterControllers(typeof(MvcApplication).Assembly).InstancePerRequest();
            cb.RegisterType<AccountController>().InstancePerDependency();
            cb.RegisterAssemblyModules(typeof(MvcApplication).Assembly);
           
            var container = cb.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

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
