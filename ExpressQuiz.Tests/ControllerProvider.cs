using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ExpressQuiz.Controllers;

namespace ExpressQuiz.Tests
{
    public class ControllerProvider
    {
        private RepoProvider _repoProvider;

        public ControllerProvider(RepoProvider repoProvider)
        {
            _repoProvider = repoProvider;
        }

      

        public QuizzesController CreateQuizzesController()
        {
            var c = new QuizzesController(
             _repoProvider.AnswerRepo,
             _repoProvider.QuestionRepo,
              _repoProvider.QuizCategoryRepo,
              _repoProvider.QuizRepo,
             _repoProvider.QuizRatingRepo,
             _repoProvider.QuizResultRepo
                );
            return c;
        }
    }
}
