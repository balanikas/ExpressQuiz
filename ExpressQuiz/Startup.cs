using System;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using ExpressQuiz.Migrations;
using ExpressQuiz.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ExpressQuiz.Startup))]
namespace ExpressQuiz
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

           // Seed(new QuizDbContext());
            //RunAdditionalSeeds(new QuizDbContext());
        }

        private void Seed(QuizDbContext context)
        {
            context.UserAnswers.RemoveRange(context.UserAnswers.AsEnumerable());
            context.QuizResults.RemoveRange(context.QuizResults.AsEnumerable());
            context.QuizCategories.RemoveRange(context.QuizCategories.AsEnumerable());

            context.SaveChanges();

            var uri = DataProvider.MapPath("~/bin/App_Data/seeddata.xml");

            var quizzes = DataProvider.Import(uri);

            context.QuizCategories.AddOrUpdate(i => i.Name,
               quizzes.Select(x => x.Category).DistinctBy(x => x.Name).ToArray()
               );



            context.SaveChanges();

            context.Quizzes.AddOrUpdate(i => i.Name,
                      quizzes.ToArray()
                 );
        }

        private void RunAdditionalSeeds(QuizDbContext context)
        {
            var uri = DataProvider.MapPath("~/App_Data/imports/out.xml");

            var quizzes = DataProvider.Import(uri);

            context.QuizCategories.AddOrUpdate(i => i.Name,
               quizzes.Select(x => x.Category).DistinctBy(x => x.Name).ToArray()
               );

            context.SaveChanges();

            foreach (var quiz in quizzes)
            {
                quiz.Category = context.QuizCategories.First(x => x.Name == quiz.Category.Name);
            }


            context.Quizzes.AddOrUpdate(i => i.Name,
                        quizzes.ToArray()
                   );
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var error in e.EntityValidationErrors)
                {
                    foreach (var error2 in error.ValidationErrors)
                    {
                        Debug.WriteLine(error2.ErrorMessage);
                    }
                }
                throw;
            }
           
        }
    }
}
