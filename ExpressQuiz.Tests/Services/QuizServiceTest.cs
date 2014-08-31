using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressQuiz.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressQuiz.Tests.Services
{
    [TestClass]
    public class QuizServiceTest :ServiceTest
    {

       

        [TestMethod]
        public void QuizService_NotExisting_ShouldReturnFalse()
        {
            var service = _mockRepo.QuizService;
            Assert.IsFalse(service.Exists(""));

        }

        [TestMethod]
        public void QuizService_Existing_ShouldReturnTrue()
        {
            var service = _mockRepo.QuizService;
            Assert.IsTrue(service.Exists("quiz 1"));

        }

        [TestMethod]
        public void QuizService_Delete()
        {
            var service = _mockRepo.QuizService;
            Assert.IsNotNull(service.Get(1));
            service.Delete(1);
            Assert.IsNull(service.Get(1));
            
        }

        [TestMethod]
        public void QuizService_Get()
        {
            var service = _mockRepo.QuizService;
            Assert.IsNotNull(service.Get(1));
            Assert.IsNull(service.Get(-1));
  
        }

        [TestMethod]
        public void QuizService_GetAll()
        {
            var service = _mockRepo.QuizService;
            Assert.IsNotNull(service.GetAll());
            

        }

        [TestMethod]
        public void QuizService_GetPublicQuizzes()
        {
            var service = _mockRepo.QuizService;

            var publicCount = service.GetPublicQuizzes().Count();

            var quiz = service.Get(1);
            quiz.Locked = true;
            service.Insert(quiz);

            var publicCountAfterUpdate = service.GetPublicQuizzes().Count();

            Assert.IsTrue(publicCount > publicCountAfterUpdate);

            
        }

        [TestMethod]
        public void QuizService_Insert()
        {
            var service = _mockRepo.QuizService;
            var quiz = new Quiz();
            quiz.Name = "666";
            service.Insert(quiz);

            Assert.IsNotNull(service.Exists("666"));

       
        }


        [TestMethod]
        public void QuizService_Update()
        {
            var service = _mockRepo.QuizService;
            var quiz = service.Get(1);
            quiz.Name = "666";
            service.Update(quiz);

            Assert.IsNotNull(service.Exists("666"));

  
        }

        [TestMethod]
        public void QuizService_GetByCategory()
        {
            var service = _mockRepo.QuizService;
            Assert.IsTrue( service.GetByCategory(1).Count() == 1);

        }

        [TestMethod]
        public void QuizService_GetByCreationDate()
        {
            var service = _mockRepo.QuizService;
            var quiz = service.Get(1);
            quiz.Created = DateTime.Now + TimeSpan.FromHours(1);
            service.Update(quiz);
            var quizzes = service.GetByCreationDate(true).ToList();

            var date1 = quizzes[0].Created;
            var date2 = quizzes[1].Created;

            Assert.IsTrue(date1 > date2);

            quizzes = service.GetByCreationDate(false).ToList();
            date1 = quizzes[0].Created;
            date2 = quizzes[1].Created;
            Assert.IsTrue(date1 < date2);
        }

        [TestMethod]
        public void QuizService_GetByRating()
        {
            var service = _mockRepo.QuizService;
            var quizzes = service.GetByRating(true).ToList();

            Assert.AreEqual("quiz 1",quizzes.First().Name);

            quizzes = service.GetByRating(false).ToList();

            Assert.AreEqual("quiz 2", quizzes.First().Name);
        }

        [TestMethod]
        public void QuizService_GetByLevel()
        {
            var service = _mockRepo.QuizService;
            var quizzes = service.GetByLevel(true).ToList();

            Assert.AreEqual("quiz 2", quizzes.First().Name);

            quizzes = service.GetByLevel(false).ToList();

            Assert.AreEqual("quiz 1", quizzes.First().Name);
        }

       
    }
}
