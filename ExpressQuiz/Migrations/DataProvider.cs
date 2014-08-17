using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Hosting;
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

            bool valid = true;
            doc.Validate(schemas, (o, e) =>
            {
                throw new Exception(e.Exception.InnerException.Message);

                valid = false;
            });

            return valid;
        }

        public static IEnumerable<Quiz> Import(string uri)
        {

          
            var xml = XDocument.Load(uri, LoadOptions.PreserveWhitespace);
            //string schemaUri = @"C:\Users\grillo\Documents\GitHub\ExpressQuiz\ExpressQuiz\App_Data\seeddata.xsd";
            //if (!ValidateData(xml,schemaUri))
            //{
            //    if (!Debugger.IsAttached)
            //    {
            //        Debugger.Launch();
            //    }
            //}

            char[] charsToTrim = { '\r', ' ', '\n' };


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
                            Text = a.Element("Text").Value.Trim(charsToTrim),
                            IsCorrect = a.Descendants("IsCorrect").Any(),
                            Explanation = a.Element("Explanation").Value.Trim(charsToTrim),
                            OrderId = int.Parse(a.Element("OrderId").Value.Trim(charsToTrim))
                        });

                    }
                    questions.Add(new Question()
                    {
                        Answers = answers,
                        Text = q.Element("Text").Value.Trim(charsToTrim),
                        OrderId = int.Parse(q.Element("OrderId").Value.Trim(charsToTrim)),
                        EstimatedTime = int.Parse(q.Element("EstimatedTime").Value.Trim(charsToTrim)),
                        Points = int.Parse(q.Element("Points").Value.Trim(charsToTrim)),

                    });
                }

                quizzes.Add(new Quiz()
                {
                    Category = new QuizCategory()
                    {
                        Name =  ((string)quiz.Attribute("category")).Trim(charsToTrim)
                    } ,
                    Name = quiz.Element("Name").Value.Trim(charsToTrim),
                    Summary = quiz.Element("Summary").Value.Trim(charsToTrim),
                    Questions = questions,
                    Created = DateTime.Now,
                    IsTimeable = quiz.Descendants("IsTimeable").Any(),
                    CreatedBy = quiz.Element("CreatedBy").Value.Trim(charsToTrim)
                    

                });

            }
            return quizzes;

        }


        private static QuizCategory GetCategory(List<Quiz> quizzes, string name)
        {
         
            return new QuizCategory()
            {
                Name = name
            };
        }


        public static void Export(List<Quiz> quizzes , string fileName)
        {
            var doc = new XDocument();
            var root = new XElement("Content");
            doc.Add(root);
            var quizzesEl = new XElement("Quizzes");
            root.Add(quizzesEl);

          

            foreach (var quiz in quizzes.ToList())
            {
                var quizEl = new XElement("Quiz", new XAttribute("category",quiz.Category.Name));
                quizEl.Add(new XElement("Name",quiz.Name));
                quizEl.Add(new XElement("Summary", quiz.Summary));
                quizEl.Add(new XElement("CreatedBy", quiz.CreatedBy));
                if (quiz.IsTimeable)
                {
                    quizEl.Add(new XElement("IsTimeable"));
                }
                foreach (var q in quiz.Questions.ToList())
                {
                    var qEl = new XElement("Question");
                    qEl.Add(new XElement("OrderId", q.OrderId));
                    qEl.Add(new XElement("Text", q.Text));
                    qEl.Add(new XElement("EstimatedTime", q.EstimatedTime));
                    qEl.Add(new XElement("Points", q.Points));
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

        public static string MapPath(string seedFile)
        {
            if (HttpContext.Current != null)
                return HostingEnvironment.MapPath(seedFile);

            var absolutePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath;
            var directoryName = Path.GetDirectoryName(absolutePath);
            var path = Path.Combine(directoryName, ".." + seedFile.TrimStart('~').Replace('/', '\\'));

            return path;
        }


    }
}