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


        public static List<Quiz> ParseSeedDataFromXml(string uri)
        {

           
            var xml = XDocument.Load(uri, LoadOptions.PreserveWhitespace);



            var content = xml.Element("Content");
            var catId = 0;
            var categories = content.Element("Categories").Elements("Category").Select(x => new QuizCategory
            {
                Name = x.Element("Name").Value,
                Id = catId++

            }).ToArray();

            var quizzes = new List<Quiz>();
            foreach (var quiz in content.Element("Quizzes").Elements("Quiz"))
            {
                var questions = new List<Question>();
                foreach (var q in quiz.Elements("Question"))
                {
                    var answers = new List<Answer>();
                    foreach (var a in q.Elements("Answer"))
                    {
                        answers.Add(new Answer()
                        {
                            Text = a.Element("Text").Value,
                            IsCorrect = a.Descendants("IsCorrect").Any(),
                            Explanation = a.Element("Explanation").Value,
                            OrderId = int.Parse(a.Element("OrderId").Value)
                        });

                    }
                    questions.Add(new Question()
                    {
                        Answers = answers,
                        Text = q.Element("Text").Value,
                        OrderId = int.Parse(q.Element("OrderId").Value)

                    });
                }

                quizzes.Add(new Quiz()
                {
                    Category = categories.First(x => x.Id == int.Parse((string)quiz.Attribute("category"))),
                    Name = quiz.Element("Name").Value,
                    Summary = quiz.Element("Summary").Value,
                    Questions = questions


                });

            }
            return quizzes;

        }

        protected override void Seed(QuizDbContext context)
        {
            

            context.UserAnswers.RemoveRange(context.UserAnswers.AsEnumerable());
            context.QuizResults.RemoveRange(context.QuizResults.AsEnumerable());

            var uri = @"C:\Users\grillo\Documents\GitHub\ExpressQuiz\ExpressQuiz\App_Data\seeddata.xml";
            var quizzes = ParseSeedDataFromXml(uri);

            context.Quizzes.AddOrUpdate(i => i.Name,
                        quizzes.ToArray()
                   );
           
        }
    }
}
