using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
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
            context.Set<UserAnswer>().RemoveRange(context.UserAnswers.AsEnumerable());
            context.Set<QuizResult>().RemoveRange(context.QuizResults.AsEnumerable());
            context.Set<QuizCategory>().RemoveRange(context.QuizCategories.AsEnumerable());

            context.SaveChanges();


            //quizzes
            var uri = DataProvider.MapPath("~/../Migrations/Quiz/seeddata.xml");

            var quizzes = DataProvider.Import(uri).ToList();



            context.Set<QuizCategory>().AddOrUpdate(i => i.Name,
               quizzes.Select(x => x.Category).DistinctBy(x => x.Name).ToArray()
               );

            context.SaveChanges();

            foreach (var quiz in quizzes)
            {
                quiz.Category = context.Set<QuizCategory>().First(x => x.Name == quiz.Category.Name);
            }

            context.Set<Models.Quiz>().AddOrUpdate(i => i.Name,
                      quizzes.ToArray()
                 );

            context.SaveChanges();


            //ratings
           AddQuizRatings(context);



            AddQuizResults(context);
          
        }

        private void AddQuizRatings(QuizDbContext ctx)
        {
            var ratings = new List<QuizRating>()
            {
                new QuizRating() {Level = 50, QuizId = 1, Rating = 80},
                new QuizRating() {Level = 30, QuizId = 1, Rating = 30},
                new QuizRating() {Level = 80, QuizId = 2, Rating = 50},
                new QuizRating() {Level = 30, QuizId = 2, Rating = 30}
            };

            ctx.Set<QuizRating>().AddRange(ratings);
        }

        private void AddQuizResults(QuizDbContext ctx)
        {

            //if (!Debugger.IsAttached)
            //{
            //    Debugger.Launch();
            //}
            var quiz = ctx.Set<Models.Quiz>().ToList()[0];
            var q1 = quiz.Questions.ToList()[0];
            var q2 = quiz.Questions.ToList()[1];

            //results
            var result1 = new QuizResult()
            {
                Answers = new List<UserAnswer>()
                {
                    new UserAnswer() {AnswerId = q1.Answers.ToList()[0].Id, QuestionId = q1.Id},
                    new UserAnswer() {AnswerId = q2.Answers.ToList()[0].Id, QuestionId = q2.Id},
                },
                EllapsedTime = 10,
                Score = 10,
                QuizId = quiz.Id,
                UserId = "user1"
            };
            var result2 = new QuizResult()
            {
                Answers = new List<UserAnswer>()
                {
                    new UserAnswer() {AnswerId = q1.Answers.ToList()[1].Id, QuestionId = q1.Id},
                    new UserAnswer() {AnswerId = q2.Answers.ToList()[1].Id, QuestionId = q2.Id},
                },
                EllapsedTime = 20,
                Score = 10,
                QuizId = quiz.Id,
                UserId = "user1"
            };

            var results = new List<QuizResult>() { result1, result2 };

            ctx.Set<QuizResult>().AddRange(results);
        }

    }
}
