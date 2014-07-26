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

        private readonly IQuizResultRepo _quizResultRepo;

        private readonly IQuestionRepo _questionRepo;
        private readonly IAnswerRepo _answerRepo;

        public QuizReviewController()
        {
            _quizResultRepo = new QuizResultRepo(new QuizDbContext());
            _questionRepo = new QuestionRepo(new QuizDbContext());
            _answerRepo = new AnswerRepo(new QuizDbContext());
        }
        // GET: QuizReview
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var result = _quizResultRepo.GetById(id.Value);
            if (result != null)
            {
                var vm = new QuizReviewViewModel(result,_answerRepo, _questionRepo);
                //vm.Score = 56;
                return View(vm);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }

        public ActionResult Question(int? questionID, int resultId )
        {
            if (questionID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var result = _questionRepo.GetById(questionID.Value);
            if (result != null)
            {
                var vm = new QuestionReviewViewModel(result,resultId);
                return View(vm);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }
    }
}