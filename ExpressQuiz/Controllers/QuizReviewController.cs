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

        private readonly IRepo<Question> _questionRepo;
        private readonly IRepo<Answer> _answerRepo;
        private readonly IRepo<QuizRating> _quizRatingRepo;

        public QuizReviewController()
        {
            var ctx = new QuizDbContext();


            _quizResultRepo = new Repo<QuizResult>(ctx);
            _questionRepo = new Repo<Question>(ctx);
            _answerRepo = new Repo<Answer>(ctx);
            _quizRatingRepo = new Repo<QuizRating>(ctx);
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

                var vm = new QuizReviewViewModel();
                vm.Items = GetQuestionDetails(result);
                vm.Result = result;
                vm.QuizId = result.QuizId;
                return View(vm);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }

        public List<QuizReviewItem> GetQuestionDetails(QuizResult result)
        {


            var qDetails = new List<QuizReviewItem>();
            foreach (var userAnswer in result.Answers)
            {
                var answer = _answerRepo.GetAll().FirstOrDefault(x => x.Id == userAnswer.AnswerId);
                var isAnswerCorrect = answer != null ? answer.IsCorrect : false;
                var questionText = _questionRepo.GetAll().First(x => x.Id == userAnswer.QuestionId).Text;

                qDetails.Add(new QuizReviewItem(isAnswerCorrect, questionText, userAnswer.QuestionId));


            }

            return qDetails;
        }

        [HttpPost]
        public ActionResult Index(QuizReviewViewModel model)
        {
            var quizRating = new QuizRating();
            quizRating.Rating = model.Rating;
            quizRating.QuizId = model.QuizId;
            _quizRatingRepo.Insert(quizRating);
            _quizRatingRepo.Save();
            return RedirectToAction("Index", "Quizzes");
        }

        public ActionResult Question(int? questionId, int resultId )
        {
            if (questionId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var result = _questionRepo.Get(questionId.Value);
            if (result != null)
            {
                var vm = new QuestionReviewViewModel(result,resultId);
                return View(vm);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }
    }
}