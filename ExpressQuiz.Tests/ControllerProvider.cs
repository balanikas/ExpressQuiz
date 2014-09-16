using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ExpressQuiz.Controllers;
using ExpressQuiz.Core.Services;
using Moq;

namespace ExpressQuiz.Tests
{
    public class ControllerProvider
    {
        private readonly MockRepository _mockRepo;
        

        public ControllerProvider(MockRepository mockRepo)
        {
            _mockRepo = mockRepo;
            
        }

        public QuizzesController CreateQuizzesController()
        {
            
            var c = new QuizzesController(
            _mockRepo.AnswerService,
            _mockRepo.QuestionService,
            _mockRepo.QuizCategoryService,
            _mockRepo.QuizService, 
            _mockRepo.UserActivityService,
            _mockRepo.ModelConverter,
            _mockRepo.ViewModelConverter);

            MockUser(c);
            return c;
        }

        public HomeController CreateHomeController()
        {
            var c = new HomeController(_mockRepo.ContactInfoRepo);
            MockUser(c);
            return c;
        }

        public ActiveQuizController CreateActiveQuizzController()
        {
            var c = new ActiveQuizController(
                _mockRepo.QuizService, 
                _mockRepo.QuizResultService,  
                _mockRepo.UserActivityService,
                _mockRepo.ModelConverter,
            _mockRepo.ViewModelConverter);
            MockUser(c);

            return c;
        }

        public QuizReviewController CreateQuizReviewController()
        {
            var c = new QuizReviewController(
                _mockRepo.QuizResultService,
                _mockRepo.QuestionService,
                _mockRepo.QuizRatingRepo,
                _mockRepo.ModelConverter);
            MockUser(c);

            return c;
        }


        private void MockUser(Controller controller)
        {
            var mock = new Mock<ControllerContext>();
            mock.SetupGet(x => x.HttpContext.User.Identity.Name).Returns("user");
            mock.SetupGet(x => x.HttpContext.Request.IsAuthenticated).Returns(true);

            controller.ControllerContext = mock.Object;
        }
         
    }
}
