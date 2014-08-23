using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Migrations;
using HtmlAgilityPack;

namespace QuizTool
{
    class Program
    {
        static void Main(string[] args)
        {
            var outPath = @"C:\Users\grillo\Documents\GitHub\ExpressQuiz\ExpressQuiz\App_Data\imports\out.xml";

            string line;
            List<Quiz> quizzes = new List<Quiz>();

        
            System.IO.StreamReader file =
               new System.IO.StreamReader(Path.Combine(Environment.CurrentDirectory,"inputs.txt"));
            while ((line = file.ReadLine()) != null)
            {
                quizzes.Add(Import(line));
                Console.WriteLine("imported from " + line);
            }

            file.Close();


            DataProvider.Export(quizzes, outPath);
 
            Console.ReadLine();
        
        }


        private static Quiz Import(string uri)
        {

            string data;
            using (WebClient client = new WebClient())
            {
                data = client.DownloadString(uri);
            }

            var doc = HtmlToXElement(data);
            
            var quizInfo = doc.Descendants().First(x => x.Attributes().Any(y => y.Value == "with-image-intro"));
            var quizCat = doc.Descendants().First(x => x.Attributes().Any(y => y.Value == "body")).Attribute("class").Value;
            var quizName = (quizInfo.FirstNode as XElement).Value;
            var quizSummary = ((quizInfo.FirstNode as XElement).NextNode as XElement).Value;
            var quiz = new Quiz();
            quiz.IsTimeable = true;
            quiz.Name = quizName;
            quiz.Summary = quizSummary;
            quiz.Created = DateTime.Now;
            quiz.CreatedBy = "ChristopherBalanikas";
            quiz.AllowPoints = true;
            quiz.Locked = false;
            quiz.Category = new QuizCategory()
            {
                Name = quizCat
            };


            var xQuestions = doc.Descendants().Where(x => x.Attributes().Any(y => y.Value.StartsWith("q_and_a_")));

            var qCount = 1;
            foreach (var q in xQuestions)
            {

                var qText = q.Descendants().First(x => x.Attributes().Any(y => y.Value.StartsWith("question"))).Value;
                var correctAnswer = q.Descendants().First(x => x.Attributes("id").Any(y => y.Value == "answer"));
                var correctAnswerId = int.Parse(correctAnswer.Attribute("data-answer").Value);
                var tip = correctAnswer.Attribute("data-tip").Value;


                var question = new Question();
                question.Text = qText;
                question.EstimatedTime = 10;
                question.OrderId = qCount++;
                question.Points = 10;


                var answers = q.Descendants().Where(x => x.Attributes().Any(y => y.Value.StartsWith("answer block")));

                var aCount = 1;
                foreach (var a in answers)
                {
                    var text = a.Value;
                    var answer = new Answer();
                    if (correctAnswerId == aCount)
                    {
                        answer.IsCorrect = true;
                        answer.Explanation = tip;
                    }
                    answer.Text = text;
                    answer.OrderId = aCount++;

                    question.Answers.Add(answer);

                }

                quiz.Questions.Add(question);
            }

            return quiz;

        }

        public static XElement HtmlToXElement(string html)
        {
            if (html == null)
                throw new ArgumentNullException("html");

            HtmlDocument doc = new HtmlDocument();
            doc.OptionOutputAsXml = true;
            doc.LoadHtml(html);
            using (StringWriter writer = new StringWriter())
            {
                doc.Save(writer);
                using (StringReader reader = new StringReader(writer.ToString()))
                {
                    return XElement.Load(reader);
                }
            }
        }
    }
}
