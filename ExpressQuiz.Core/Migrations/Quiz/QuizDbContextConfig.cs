using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Utils;
using Microsoft.Ajax.Utilities;

namespace ExpressQuiz.Core.Migrations.Quiz
{
   

    internal sealed class QuizDbContextConfig : DbMigrationsConfiguration<QuizDbContext>
    {
        public QuizDbContextConfig()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(QuizDbContext context)
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

            AddLotsOfQuizzes(context,quizzes[0]);


            context.Set<Models.Quiz>().AddOrUpdate(i => i.Name,
                      quizzes.ToArray()
                 );

            context.SaveChanges();

            AddQuizRatings(context);

            AddQuizResults(context);
          
        }


        private void AddLotsOfQuizzes(QuizDbContext ctx, Models.Quiz reference)
        {
            
            for (int k = 0; k < 100; k++)
            {
                var quizTemp = new Models.Quiz()
                {
                    AllowPoints = true,
                    Category = reference.Category,
                    Created = DateTime.Now,
                    CreatedBy = "info@coderentals.com",
                    IsTimeable = true,
                    Locked = false,
                    Name = reference.Name + "-" + k,
                    Summary = "sum" + k,

                };

                for (int i = 0; i < 100; i++)
                {
                    var q = new Question()
                    {
                        OrderId = i,
                        Text = "text",
                        EstimatedTime = 10,
                        Points = 5,

                    };
                    for (int j = 0; j < 50; j++)
                    {
                        q.Answers.Add(new Answer()
                        {
                            Explanation = "",
                            Text = "text",
                            OrderId = j,

                        });
                    }

                    quizTemp.Questions.Add(q);
                }



                ctx.Set<Models.Quiz>().AddOrUpdate(i => i.Name,
                    quizTemp
                );
                ctx.SaveChanges();
            }
           
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
