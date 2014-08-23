using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ExpressQuiz.Controllers;
using ExpressQuiz.Core.Services;

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
            _mockRepo.QuizRatingRepo,
            _mockRepo.QuizResultRepo,
            _mockRepo.QuizService);

            return c;
        }

        public HomeController CreateHomeController()
        {
            var c = new HomeController(_mockRepo.ContactInfoRepo);
            return c;
        }

        public ActiveQuizController CreateActiveQuizzController()
        {
            var c = new ActiveQuizController(
                _mockRepo.QuizService, 
                _mockRepo.QuizResultRepo, 
                _mockRepo.AnswerService,
                _mockRepo.QuestionService);

            return c;
        }

        public QuizReviewController CreateQuizReviewController()
        {
            var c = new QuizReviewController(
                _mockRepo.QuizService,
                _mockRepo.QuizResultRepo,
                _mockRepo.QuestionService,
                _mockRepo.AnswerService,
                _mockRepo.QuizRatingRepo);

            return c;
        }

         
    }
}
