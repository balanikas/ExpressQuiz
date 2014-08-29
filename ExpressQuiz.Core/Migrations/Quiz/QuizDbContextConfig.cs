using System.Data.Entity.Migrations;
using System.Linq;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Utils;
using Microsoft.Ajax.Utilities;

namespace ExpressQuiz.Core.Migrations.Quiz
{
   

    internal sealed class QuizDbContextConfig : DbMigrationsConfiguration<ExpressQuiz.Core.Models.QuizDbContext>
    {
        public QuizDbContextConfig()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ExpressQuiz.Core.Models.QuizDbContext context)
        {
            //context.Set<UserAnswer>().RemoveRange(context.UserAnswers.AsEnumerable());
            //context.Set<QuizResult>().RemoveRange(context.QuizResults.AsEnumerable());
            //context.Set<QuizCategory>().RemoveRange(context.QuizCategories.AsEnumerable());

            //context.SaveChanges();

            

            //var uri = DataProvider.MapPath("~/../Migrations/Quiz/seeddata.xml");

            //var quizzes = DataProvider.Import(uri).ToList();



            //context.Set<QuizCategory>().AddOrUpdate(i => i.Name,
            //   quizzes.Select(x => x.Category).DistinctBy(x => x.Name).ToArray()
            //   );

            //context.SaveChanges();

            //foreach (var quiz in quizzes)
            //{
            //    quiz.Category = context.Set<QuizCategory>().First(x => x.Name == quiz.Category.Name);
            //}

            //context.Set<Models.Quiz>().AddOrUpdate(i => i.Name,
            //          quizzes.ToArray()
            //     );

            //context.SaveChanges();
        }
    }
}
