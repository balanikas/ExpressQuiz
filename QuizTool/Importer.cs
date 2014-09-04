using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ExpressQuiz.Core.Models;

namespace QuizTool
{
    class Importer
    {
        public Quiz Import(XElement doc)
        {
           
            var quizInfo = doc.Descendants().First(x => x.Attributes().Any(y => y.Value == "with-image-intro"));
            var quizCat = doc.Descendants().First(x => x.Attributes().Any(y => y.Value == "body")).Attribute("class").Value;
            var quizName = (quizInfo.FirstNode as XElement).Value;
            var quizSummary = ((quizInfo.FirstNode as XElement).NextNode as XElement).Value;


            var quiz = new Quiz
            {
                IsTimeable = true,
                Name = quizName,
                Summary = quizSummary,
                Created = DateTime.Now,
                CreatedBy = "ChristopherBalanikas",
                AllowPoints = true,
                Locked = false,
                Category = new QuizCategory()
                {
                    Name = quizCat
                }
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

        public List<Quiz> Import(string source, string destination)
        {

            var quizzes = new List<Quiz>();
            var files = Directory.EnumerateFiles(source);

            foreach (var file in files)
            {
                var data = Helpers.ReadDocument(file);

                

                try
                {
                    var doc = Helpers.HtmlToXElement(data);
                    var quiz = Import(doc);

                    if (!Helpers.ValidateQuiz(quiz))
                    {
                        Trace.WriteLine("quiz is not valid!  " + file);
                    }
                    else
                    {
                        quizzes.Add(quiz);
                        Trace.WriteLine("imported from " + file);
                    }

                  
                   
                    
                }
                catch (Exception ex)
                {

                    Trace.WriteLine("failed to import from " + file);
                    Trace.WriteLine("reason: " + ex.Message);
                }
               
            }

            return quizzes;
        }

    }
}
