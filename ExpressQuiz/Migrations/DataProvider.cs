using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using ExpressQuiz.Models;
using ExpressQuiz.Repos;

namespace ExpressQuiz.Migrations
{
    public class DataProvider
    {

        private static bool ValidateData(XDocument doc, string schemaUri)
        {
            var schemas = new XmlSchemaSet();
            schemas.Add("", schemaUri);

            bool errors = false;
            doc.Validate(schemas, (o, e) =>
            {
                Console.WriteLine("{0}", e.Message);
                errors = true;
            });

            return errors;
        }

        public static List<Quiz> Import(QuizDbContext context, string uri)
        {

            var xml = XDocument.Load(uri, LoadOptions.PreserveWhitespace);
            string schemaUri = @"C:\Users\grillo\Documents\GitHub\ExpressQuiz\ExpressQuiz\App_Data\seeddata.xsd";
            if (!ValidateData(xml,schemaUri))
            {
                if (!Debugger.IsAttached)
                {
                    Debugger.Launch();
                }
            }


            var content = xml.Element("Content");

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
                    Category = new QuizCategory() { Name = (string)quiz.Attribute("category") },
                    Name = quiz.Element("Name").Value,
                    Summary = quiz.Element("Summary").Value,
                    Questions = questions


                });

            }
            return quizzes;

        }

        internal static void Export(IRepo<Quiz> repo, string fileName)
        {
            var doc = new XDocument();
            var root = new XElement("Content");
            doc.Add(root);
            var quizzesEl = new XElement("Quizzes");
            root.Add(quizzesEl);

            var quizzes = from m in repo.GetAll()
                          select m;

            foreach (var quiz in quizzes.ToList())
            {
                var quizEl = new XElement("Quiz", new XAttribute("category",quiz.Category.Name));
                quizEl.Add(new XElement("Name",quiz.Name));
                quizEl.Add(new XElement("Summary", quiz.Summary));

                foreach (var q in quiz.Questions.ToList())
                {
                    var qEl = new XElement("Question");
                    qEl.Add(new XElement("OrderId", q.OrderId));
                    qEl.Add(new XElement("Text", q.Text));

                    foreach (var a in q.Answers.ToList())
                    {
                        var aEl = new XElement("Answer");
                        aEl.Add(new XElement("OrderId", a.OrderId));
                        aEl.Add(new XElement("Text", a.Text));
                        aEl.Add(new XElement("Explanation", a.Explanation));
                        if (a.IsCorrect)
                        {
                            aEl.Add(new XElement("IsCorrect"));
                        }

                        qEl.Add(aEl);
                    }
                    quizEl.Add(qEl);
                }
                quizzesEl.Add(quizEl);
            }

            doc.Save(fileName);

        }


    }
}