using System.Collections.Generic;
using System.Xml.Linq;
using ExpressQuiz.Models;
using System.Data.Entity.Migrations;
using System.Linq;

namespace ExpressQuiz.Migrations
{



    internal sealed class Configuration : DbMigrationsConfiguration<QuizDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(QuizDbContext context)
        {


            context.UserAnswers.RemoveRange(context.UserAnswers.AsEnumerable());
            context.QuizResults.RemoveRange(context.QuizResults.AsEnumerable());

            var uri = @"C:\Users\grillo\Documents\GitHub\ExpressQuiz\ExpressQuiz\App_Data\seeddata.xml";
            var quizzes = DataProvider.Import(context, uri);

            context.Quizzes.AddOrUpdate(i => i.Name,
                        quizzes.ToArray()
                   );

        }
      
    }
}
