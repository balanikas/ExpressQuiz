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
    public class AnswerServiceTests : ServiceTest
    {
        [TestMethod]
        public void AnswerService_GetAll()
        {
            var service = _mockRepo.AnswerService;
            Assert.IsNotNull(service.GetAll());
        }

        [TestMethod]
        public void AnswerService_Get()
        {
            var service = _mockRepo.AnswerService;
            var answer = service.Get(1);
            Assert.IsNotNull(answer);
        }

        [TestMethod]
        public void AnswerService_Insert()
        {
            var service = _mockRepo.AnswerService;

            var answer = new Answer();
            answer.IsCorrect = true;
            answer.OrderId = 10;
            answer.Explanation = "exp";
            answer.Text = "text";

            var saved = service.Insert(answer);

            Assert.AreEqual(answer.IsCorrect, saved.IsCorrect);
            Assert.AreEqual(answer.OrderId, saved.OrderId);
            Assert.AreEqual(answer.Explanation, saved.Explanation);
            Assert.AreEqual(answer.Text, saved.Text);
            Assert.IsTrue(saved.Id > 0);
        }

        [TestMethod]
        public void AnswerService_Delete()
        {
            var service = _mockRepo.AnswerService;
            service.Delete(1);
            Assert.IsNull(service.Get(1));
        }

        [TestMethod]
        public void AnswerService_Update()
        {
            var service = _mockRepo.AnswerService;

            var answer = service.Get(1);
            answer.IsCorrect = true;
            answer.OrderId = 10;
            answer.Explanation = "exp";
            answer.Text = "text";

            service.Update(answer);

            var updated = service.Get(1);

            Assert.AreEqual(answer.IsCorrect, updated.IsCorrect);
            Assert.AreEqual(answer.OrderId, updated.OrderId);
            Assert.AreEqual(answer.Explanation, updated.Explanation);
            Assert.AreEqual(answer.Text, updated.Text);

        }

        [TestMethod]
        public void AnswerService_SaveOrder()
        {
            var service = _mockRepo.AnswerService;

         
            service.SaveOrder(_mockRepo.QuestionService.Get(1), "2,1");

            var updated = service.GetAll().Where(x => x.QuestionId == 1).ToList();

            Assert.AreEqual(0, updated[1].OrderId);
            Assert.AreEqual(1, updated[0].OrderId);
        }

        [TestMethod]
        public void AnswerService_SaveOrder_InvalidOrder()
        {
            var service = _mockRepo.AnswerService;

            try
            {
                service.SaveOrder(_mockRepo.QuestionService.Get(1), "hello,2,1,6");
                Assert.Fail();
            }
            catch
            {
                
            }

            try
            {
                service.SaveOrder(_mockRepo.QuestionService.Get(1), "1,2,3,4,5,6,7,8,9,10");
                Assert.Fail();
            }
            catch
            {

            }
           
        }

    }
}
