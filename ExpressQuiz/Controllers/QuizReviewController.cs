using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Repos;
using ExpressQuiz.Core.Services;
using ExpressQuiz.Extensions;
using ExpressQuiz.ViewModels;

namespace ExpressQuiz.Controllers
{
    public class QuizReviewController : Controller
    {
        private readonly IQuizService _quizService;
        private readonly IRepo<QuizResult> _quizResultRepo;
        private readonly IQuestionService _questionService;
        private readonly IAnswerService _answerService;
        private readonly IRepo<QuizRating> _quizRatingRepo;

        public QuizReviewController(
            IQuizService quizService,
            IRepo<QuizResult> quizResultRepo,
            IQuestionService questionService,
            IAnswerService answerService,
            IRepo<QuizRating> quizRatingRepo

            )
        {
            _quizService = quizService;
            _quizResultRepo = quizResultRepo;
            _questionService = questionService;
            _answerService = answerService;
            _quizRatingRepo = quizRatingRepo;
        }

        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var result = _quizResultRepo.Get(id.Value);
            if (result != null)
            {

                var vm = result.ToViewModel(_quizService, _answerService);
                return View("Index",vm);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }

     

        [HttpPost]
        public ActionResult Index(QuizReviewViewModel model)
        {
            var quizRating = new QuizRating();
            quizRating.Rating = model.Rating;
            quizRating.Level = model.Level * 20;
            quizRating.QuizId = model.QuizId;
            _quizRatingRepo.Insert(quizRating);
            _quizRatingRepo.Save();
            return RedirectToAction("Index", "Quizzes");
        }

        public ActionResult Question(int? questionId, int resultId, int userAnswerId )
        {
            if (questionId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var q = _questionService.Get(questionId.Value);
            if (q != null)
            {
                var vm = q.ToViewModel(resultId, userAnswerId);
                return View("Question",vm);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }
    }
}