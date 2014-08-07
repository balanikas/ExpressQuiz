namespace ExpressQuiz.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ExpressQuiz.Models.QuizDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ExpressQuiz.Models.QuizDbContext context)
        {


            context.UserAnswers.RemoveRange(context.UserAnswers.AsEnumerable());
            context.QuizResults.RemoveRange(context.QuizResults.AsEnumerable());
            context.QuizCategories.RemoveRange(context.QuizCategories.AsEnumerable());

            var uri = @"C:\Users\grillo\Documents\GitHub\ExpressQuiz\ExpressQuiz\App_Data\seeddata.xml";
            var quizzes = DataProvider.Import(context, uri);

            context.Quizzes.AddOrUpdate(i => i.Name,
                        quizzes.ToArray()
                   );
        }
    }
}
