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

        public ControllerProvider(RepoProvider repoProvider)
        {
            _repoProvider = repoProvider;
        }

      

        public QuizzesController CreateQuizzesController()
        {
            var quizService = new QuizService(_repoProvider.QuizRepo, _repoProvider.QuizResultRepo,
                _repoProvider.QuizRatingRepo);

            var c = new QuizzesController(
               
             _repoProvider.AnswerRepo,
             _repoProvider.QuestionRepo,
              _repoProvider.QuizCategoryRepo,
             _repoProvider.QuizRatingRepo,
             _repoProvider.QuizResultRepo,
              quizService
                );
            return c;
        }
    }
}
