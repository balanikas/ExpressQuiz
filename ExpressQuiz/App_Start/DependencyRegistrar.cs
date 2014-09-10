using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using ExpressQuiz.Controllers;
using ExpressQuiz.Core;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Repos;
using ExpressQuiz.Core.Services;

namespace ExpressQuiz
{
    public class DependencyRegistrar
    {
        public static void Register()
        {
            var cb = new Autofac.ContainerBuilder();

            cb.RegisterType<QuizDbContext>().InstancePerRequest();
            cb.RegisterType<Repo<Quiz>>().As<IRepo<Quiz>>().InstancePerRequest();
            cb.RegisterType<Repo<Question>>().As<IRepo<Question>>().InstancePerRequest();
            cb.RegisterType<Repo<Answer>>().As<IRepo<Answer>>().InstancePerRequest();
            cb.RegisterType<Repo<QuizCategory>>().As<IRepo<QuizCategory>>().InstancePerRequest();
            cb.RegisterType<Repo<QuizRating>>().As<IRepo<QuizRating>>().InstancePerRequest();
            cb.RegisterType<Repo<QuizResult>>().As<IRepo<QuizResult>>().InstancePerRequest();
            cb.RegisterType<Repo<ContactInfo>>().As<IRepo<ContactInfo>>().InstancePerRequest();
            cb.RegisterType<Repo<UserActivity>>().As<IRepo<UserActivity>>().InstancePerRequest();

            cb.RegisterType<QuizService>().As<IQuizService>().InstancePerRequest();
            cb.RegisterType<QuestionService>().As<IQuestionService>().InstancePerRequest();
            cb.RegisterType<AnswerService>().As<IAnswerService>().InstancePerRequest();
            cb.RegisterType<QuizCategoryService>().As<IQuizCategoryService>().InstancePerRequest();
            cb.RegisterType<QuizResultService>().As<IQuizResultService>().InstancePerRequest();
            cb.RegisterType<UserActivityService>().As<IUserActivityService>().InstancePerRequest();


            cb.RegisterControllers(typeof(MvcApplication).Assembly).InstancePerRequest();
            cb.RegisterType<AccountController>().InstancePerDependency();
            cb.RegisterAssemblyModules(typeof(MvcApplication).Assembly);

            var container = cb.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
    }
}