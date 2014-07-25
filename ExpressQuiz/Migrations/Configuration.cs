using System.Collections.Generic;
using System.Xml.Linq;
using ExpressQuiz.Models;

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


        public static List<Quiz> ParseSeedData()
        {

            //if (System.Diagnostics.Debugger.IsAttached == false)
            //    System.Diagnostics.Debugger.Launch();
            var xml = System.Xml.Linq.XDocument.Load(
@"C:\Users\grillo\documents\visual studio 2013\Projects\ExpressQuiz\ExpressQuiz\App_Data\seeddata.xml", LoadOptions.PreserveWhitespace);



            var content = xml.Element("Content");
            var catId = 0;
            var categories = content.Element("Categories").Elements("Category").Select(x => new ExpressQuiz.Models.QuizCategory
            {
                Name = x.Element("Name").Value,
                Id = catId++

            }).ToArray();

            var quizzes = new List<ExpressQuiz.Models.Quiz>();
            foreach (var quiz in content.Element("Quizzes").Elements("Quiz"))
            {
                var questions = new List<ExpressQuiz.Models.Question>();
                foreach (var q in quiz.Elements("Question"))
                {
                    var answers = new List<ExpressQuiz.Models.Answer>();
                    foreach (var a in q.Elements("Answer"))
                    {
                        answers.Add(new ExpressQuiz.Models.Answer()
                        {
                            Text = a.Element("Text").Value,
                            IsCorrect = a.Descendants("IsCorrect").Any(),
                            Explanation = a.Element("Explanation").Value
                        });

                    }
                    questions.Add(new ExpressQuiz.Models.Question()
                    {
                        Answers = answers,
                        Text = q.Element("Text").Value
                        

                    });
                }

                quizzes.Add(new ExpressQuiz.Models.Quiz()
                {
                    Category = categories.First(x => x.Id == int.Parse((string)quiz.Attribute("category"))),
                    Name = quiz.Element("Name").Value,
                    Summary = quiz.Element("Summary").Value,
                    Questions = questions


                });

            }
            return quizzes;

        }

        protected override void Seed(ExpressQuiz.Models.QuizDbContext context)
        {
            

            context.UserAnswers.RemoveRange(context.UserAnswers.AsEnumerable());
            context.QuizResults.RemoveRange(context.QuizResults.AsEnumerable());
            var quizzes = ParseSeedData();

            context.Quizzes.AddOrUpdate(i => i.Name,
                        quizzes.ToArray()
                   );

        }
    }
}
