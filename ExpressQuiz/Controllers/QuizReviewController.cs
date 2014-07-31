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

        public QuizReviewController()
        {
            var ctx = new QuizDbContext();


            _quizResultRepo = new Repo<QuizResult>(ctx);
            _questionRepo = new Repo<Question>(ctx);
            _answerRepo = new Repo<Answer>(ctx);
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
                var vm = new QuizReviewViewModel(result);
                //vm.Score = 56;
                return View(vm);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
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