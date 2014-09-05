using System;
using System.Linq;
using ExpressQuiz.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressQuiz.Tests.Services
{
    [TestClass]
    public class QuestionServiceTests : ServiceTest
    {
        [TestMethod]
        public void QuestionService_GetAll()
        {
            var service = _mockRepo.QuestionService;

            Assert.IsNotNull( service.GetAll());
        }

        [TestMethod]
        public void QuestionService_Get()
        {
            var service = _mockRepo.QuestionService;

            var question = service.Get(1);
            Assert.IsNotNull(question);

        }

        [TestMethod]
        public void QuestionService_Insert()
        {
            var service = _mockRepo.QuestionService;

            var question = new Question();
            question.EstimatedTime = 10;
            question.OrderId = 10;
            question.Points = 10;
            question.Text = "text";

            var saved = service.Insert(question);

            Assert.AreEqual(question.EstimatedTime, saved.EstimatedTime);
            Assert.AreEqual(question.OrderId, saved.OrderId);
            Assert.AreEqual(question.Points, saved.Points);
            Assert.AreEqual(question.Text, saved.Text);
            Assert.IsTrue(saved.Id > 0);


        }

        [TestMethod]
        public void QuestionService_Delete()
        {
            var service = _mockRepo.QuestionService;

            service.Delete(1);
            Assert.IsNull(service.Get(1));
        }

        [TestMethod]
        public void QuestionService_Update()
        {
            var service = _mockRepo.QuestionService;

            var question = service.Get(1);
            question.EstimatedTime = 10;
            question.OrderId = 10;
            question.Points = 10;
            question.Text = "text";
            service.Update(question);

            var updated = service.Get(1);

            Assert.AreEqual(question.EstimatedTime, updated.EstimatedTime);
            Assert.AreEqual(question.OrderId, updated.OrderId);
            Assert.AreEqual(question.Points, updated.Points);
            Assert.AreEqual(question.Text, updated.Text);
        }


        [TestMethod]
        public void QuestionService_SaveOrder()
        {
            var service = _mockRepo.QuestionService;

            var questions = service.GetAll().Where(x=> x.QuizId == 1).ToList();
            service.SaveOrder(1, "2,1");

            var updated = service.GetAll().Where(x=> x.QuizId == 1).ToList();

            Assert.AreEqual(0,updated[1].OrderId);
            Assert.AreEqual(1,updated[0].OrderId);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AnswerService_SaveOrder_InvalidOrder()
        {
            var service = _mockRepo.AnswerService;

            var questions = service.GetAll().Where(x => x.QuestionId == 1).ToList();

            service.SaveOrder(1, "hello,2,1,6");
        }
    }
}
