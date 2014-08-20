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
        private readonly RepoProvider _repoProvider;
        private readonly ServiceProvider _serviceProvider;

        public ControllerProvider(RepoProvider repoProvider, ServiceProvider serviceProvider)
        {
            _repoProvider = repoProvider;
            _serviceProvider = serviceProvider;
        }


        public QuizzesController CreateQuizzesController()
        {
            

            var c = new QuizzesController(

            _serviceProvider.AnswerService,
            _serviceProvider.QuestionService,
            _serviceProvider.QuizCategoryService,
            _repoProvider.QuizRatingRepo,
            _repoProvider.QuizResultRepo,
            _serviceProvider.QuizService
                );
            return c;
        }
    }
}
