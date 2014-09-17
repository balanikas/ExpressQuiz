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
    public class UserActivityServiceTests : ServiceTest
    {

        [TestMethod]
        public void UserActivityService_Add()
        {
            var service = Mocks.UserActivityService;

            service.Add("1",ActivityItem.Quiz,ActivityAction.Create,1);

            var result = service.GetAll("1");
            Assert.AreEqual(1,result.Count());

        }

        [TestMethod]
        public void UserActivityService_Delete()
        {
            var service = Mocks.UserActivityService;

            service.Add("1", ActivityItem.Quiz, ActivityAction.Create, 1);

            service.Delete("1",ActivityItem.Quiz,1);
            var result = service.GetAll("1");
            Assert.AreEqual(0, result.Count());

        }

        [TestMethod]
        public void UserActivityService_DeleteAll()
        {
            var service = Mocks.UserActivityService;

            service.Add("1", ActivityItem.Quiz, ActivityAction.Create, 1);
            service.Add("1", ActivityItem.Quiz, ActivityAction.Create, 2);

            service.DeleteAll("1");
            var result = service.GetAll("1");
            Assert.AreEqual(0, result.Count());
        }


        [TestMethod]
        public void UserActivityService_GetAll()
        {
            var service = Mocks.UserActivityService;

            service.Add("1", ActivityItem.Quiz, ActivityAction.Create, 1);
            service.Add("1", ActivityItem.Quiz, ActivityAction.Edit, 1);
            service.Add("1", ActivityItem.Question, ActivityAction.Create, 1);
            service.Add("1", ActivityItem.Question, ActivityAction.Edit, 1);
            service.Add("2", ActivityItem.Quiz, ActivityAction.Create, 2);

            Assert.AreEqual(4, service.GetAll("1").Count());
            Assert.AreEqual(2, service.GetAll("1", ActivityItem.Quiz).Count());
            Assert.AreEqual(1, service.GetAll("1", ActivityItem.Quiz, ActivityAction.Create).Count());


        }


        [TestMethod]
        public void UserActivityService_Delete_One()
        {
            var service = Mocks.UserActivityService;

            service.Add("1", ActivityItem.Quiz, ActivityAction.Create, 1);
            service.Delete("1", ActivityItem.Quiz, ActivityAction.Create, 1);

            Assert.AreEqual(0, service.GetAll("1").Count());
        }

        [TestMethod]
        public void UserActivityService_QuizVoting_VoteUpAndDown()
        {
            var service = Mocks.UserActivityService;

            var voteCountBefore = Mocks.QuizService.Get(1).Votes;
            
            service.UpdateVote("1",ActivityItem.Quiz, 1,1);

          
            Assert.AreEqual(voteCountBefore + 1, Mocks.QuizService.Get(1).Votes);

            service.UpdateVote("1", ActivityItem.Quiz, 1, -1);

            Assert.AreEqual(voteCountBefore, Mocks.QuizService.Get(1).Votes);
        }

        [TestMethod]
        public void UserActivityService_QuestionVoting_VoteUpAndDown()
        {
            var service = Mocks.UserActivityService;

            var voteCountBefore = Mocks.QuestionService.Get(1).Votes;

            service.UpdateVote("1", ActivityItem.Question, 1, 1);


            Assert.AreEqual(voteCountBefore + 1, Mocks.QuestionService.Get(1).Votes);

            service.UpdateVote("1", ActivityItem.Question, 1, -1);

            Assert.AreEqual(voteCountBefore, Mocks.QuestionService.Get(1).Votes);
        }

        [TestMethod]
        public void UserActivityService_QuizViews()
        {
            var service = Mocks.UserActivityService;

            var viewCountBefore = Mocks.QuizService.Get(1).Views;

            service.Add("1", ActivityItem.Quiz, ActivityAction.View, 1);

            Assert.AreEqual(viewCountBefore + 1, Mocks.QuizService.Get(1).Views);

        }

        [TestMethod]
        public void UserActivityService_QuizCompletions()
        {
            var service = Mocks.UserActivityService;

            var completionCountBefore = Mocks.QuizService.Get(1).Completed;

            service.Add("1", ActivityItem.Quiz, ActivityAction.EndQuiz, 1);

            Assert.AreEqual(completionCountBefore + 1, Mocks.QuizService.Get(1).Completed);

        }

        [TestMethod]
        public void UserActivityService_Exists()
        {
            var service = Mocks.UserActivityService;
            service.Add("1",ActivityItem.Answer, ActivityAction.Create,1);
            
            Assert.IsTrue( service.Exists("1",ActivityItem.Answer, ActivityAction.Create,1));
        }
    }
}
