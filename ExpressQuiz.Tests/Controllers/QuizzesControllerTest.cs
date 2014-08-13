using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ExpressQuiz.Controllers;
using ExpressQuiz.Models;
using ExpressQuiz.Repos;
using ExpressQuiz.Tests.Repos;
using ExpressQuiz.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ExpressQuiz.Tests.Controllers
{
    [TestClass]
    public class QuizzesControllerTest
    {
        [TestMethod]
        public void Index()
        {

            var questionRepo = new Mock<IRepo<Question>>();
            List<Question> questions = new List<Question>();
            questions.Add(new Question()
            {
                Id = 13
            });

            questionRepo.Setup(r => r.GetAll()).Returns(questions.AsQueryable);
            questionRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(questions.First());

            var ctx = new QuizDbContext();
            var c = new QuizzesController(
                new Repo<Answer>(ctx),
                questionRepo.Object,
                new Repo<QuizCategory>(ctx),
                new InMemQuizRepo(), 
                new Repo<QuizRating>(ctx),
                new Repo<QuizResult>(ctx)
               
                
                );

            var result = c.EditQuestion(0) as PartialViewResult;

            
            var model = result.Model as EditQuestionViewModel;
            Assert.AreEqual(13,model.Question.Id);

        }


        

    }
}
