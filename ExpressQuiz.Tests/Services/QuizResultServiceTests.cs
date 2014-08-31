using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressQuiz.Tests.Services
{
    [TestClass]
    public class QuizResultServiceTests : ServiceTest
    {

        [TestMethod]
        public void QuizResultService_GetAverageLevel()
        {
            var service = _mockRepo.QuizService;
            var quiz = service.Get(1);
            Assert.AreEqual(40, _mockRepo.QuizResultService.GetAverageLevel(quiz.Id));

        }

        [TestMethod]
        public void QuizResultService_GetAverageRating()
        {
            var service = _mockRepo.QuizService;
            var quiz = service.Get(1);
            Assert.AreEqual(55, _mockRepo.QuizResultService.GetAverageRating(quiz.Id));

        }

        [TestMethod]
        public void QuizResultService_GetAverageScore()
        {
            var service = _mockRepo.QuizService;
            var quiz = service.Get(1);
            Assert.AreEqual(65, _mockRepo.QuizResultService.GetAverageScore(quiz.Id));

            quiz = service.Get(2);
            Assert.AreEqual(-1, _mockRepo.QuizResultService.GetAverageScore(quiz.Id));
        }

        [TestMethod]
        public void QuizResultService_GetAverageTime()
        {
            var service = _mockRepo.QuizService;
            var quiz = service.Get(1);
            Assert.AreEqual(15, _mockRepo.QuizResultService.GetAverageTime(quiz.Id));

            quiz = service.Get(2);
            Assert.AreEqual(-1, _mockRepo.QuizResultService.GetAverageTime(quiz.Id));
        }

        [TestMethod]
        public void QuizResultService_GetRelativeScore()
        {
            var service = _mockRepo.QuizService;
            var quiz = service.Get(1);
            Assert.AreEqual(50, _mockRepo.QuizResultService.GetRelativeScore(quiz.Id, 60));
            Assert.AreEqual(0, _mockRepo.QuizResultService.GetRelativeScore(quiz.Id, 10));
            Assert.AreEqual(100, _mockRepo.QuizResultService.GetRelativeScore(quiz.Id, 100));
        }

        [TestMethod]
        public void QuizResultService_GetRelativeTime()
        {
            var service = _mockRepo.QuizService;
            var quiz = service.Get(1);
            Assert.AreEqual(50, _mockRepo.QuizResultService.GetRelativeTime(quiz.Id, 15));
            Assert.AreEqual(100, _mockRepo.QuizResultService.GetRelativeTime(quiz.Id, 5));
            Assert.AreEqual(0, _mockRepo.QuizResultService.GetRelativeTime(quiz.Id, 30));
        }

    }
}
