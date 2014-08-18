using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExpressQuiz.Models;
using ExpressQuiz.Repos;
using ExpressQuiz.ViewModels;

namespace ExpressQuiz.Controllers
{
    public class QuizReviewController : Controller
    {

        private readonly IRepo<QuizResult> _quizResultRepo;
        private readonly IRepo<Quiz> _quizRepo;
        private readonly IRepo<Question> _questionRepo;
        private readonly IRepo<Answer> _answerRepo;
        private readonly IRepo<QuizRating> _quizRatingRepo;

        public QuizReviewController(
            IRepo<QuizResult> quizResultRepo,
            IRepo<Quiz> quizRepo,
            IRepo<Question> questionRepo,
            IRepo<Answer> answerRepo,
            IRepo<QuizRating> quizRatingRepo

            )
        {
            _quizResultRepo = quizResultRepo;
            _quizRepo = quizRepo;
            _questionRepo = questionRepo;
            _answerRepo = answerRepo;
            _quizRatingRepo = quizRatingRepo;
        }
        // GET: QuizReview
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var result = _quizResultRepo.Get(id.Value);
            if (result != null)
            {

                var vm = result.ToViewModel(_quizRepo, _answerRepo);
                return View(vm);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }

     

        [HttpPost]
        public ActionResult Index(QuizReviewViewModel model)
        {
            var quizRating = new QuizRating();
            quizRating.Rating = model.Rating;
            quizRating.Level = model.Level;
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

            var q = _questionRepo.Get(questionId.Value);
            if (q != null)
            {
                var vm = q.ToViewModel(resultId, userAnswerId);
                return View(vm);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }
    }
}