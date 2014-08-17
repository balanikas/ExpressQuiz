using System.Data.Entity.Migrations;
using System.Linq;
using ExpressQuiz.Models;
using Microsoft.Ajax.Utilities;

namespace ExpressQuiz.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ExpressQuiz.Models.QuizDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ExpressQuiz.Models.QuizDbContext context)
        {
            //context.UserAnswers.RemoveRange(context.UserAnswers.AsEnumerable());
            //context.QuizResults.RemoveRange(context.QuizResults.AsEnumerable());
            //context.QuizCategories.RemoveRange(context.QuizCategories.AsEnumerable());

            //context.SaveChanges();

            //var uri = DataProvider.MapPath("~/bin/App_Data/seeddata.xml");

            //var quizzes = DataProvider.Import(uri);

            //context.QuizCategories.AddOrUpdate(i => i.Name,
            //   quizzes.Select(x => x.Category).DistinctBy(x => x.Name).ToArray()
            //   );


          
            //context.SaveChanges();

            //context.Quizzes.AddOrUpdate(i => i.Name,
            //          quizzes.ToArray()
            //     );
            
        }

        
    }
}
