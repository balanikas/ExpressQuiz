using System.Net;
using System.Web.Mvc;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Repos;
using ExpressQuiz.Core.Services;
using ExpressQuiz.ViewModels;

namespace ExpressQuiz.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    public class QuizReviewController : Controller
    {
        private readonly ModelConverter _modelConverter;
        private readonly IQuestionService _questionService;
        private readonly IRepo<QuizRating> _quizRatingRepo;
        private readonly IQuizResultService _quizResultService;

        public QuizReviewController(
            IQuizResultService quizResultService,
            IQuestionService questionService,
            IRepo<QuizRating> quizRatingRepo, ModelConverter modelConverter)
        {
            _quizResultService = quizResultService;
            _questionService = questionService;
            _quizRatingRepo = quizRatingRepo;
            _modelConverter = modelConverter;
        }

        [HttpGet]
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            QuizResult result = _quizResultService.Get(id.Value);

            if (result != null)
            {
                var vm = _modelConverter.ToQuizReviewViewModel(result);
                return View("Index", vm);
            }

            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }


        [HttpPost]
        public ActionResult Index(QuizReviewViewModel model)
        {
            var quizRating = new QuizRating();
            quizRating.Rating = model.Rating*20;
            quizRating.Level = model.Level*20;
            quizRating.QuizId = model.QuizId;
            _quizRatingRepo.Insert(quizRating);
            _quizRatingRepo.Save();
            return RedirectToAction("Index", "Quizzes");
        }


        public ActionResult Question(int? questionId, int resultId, int userAnswerId)
        {
            if (questionId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var q = _questionService.Get(questionId.Value);
            if (q != null)
            {
                var vm = _modelConverter.ToQuestionReviewViewModel(q, resultId, userAnswerId);
                return View("Question", vm);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }
    }
}