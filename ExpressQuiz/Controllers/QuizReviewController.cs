using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExpressQuiz.Models;
using ExpressQuiz.ViewModels;

namespace ExpressQuiz.Controllers
{
    public class QuizReviewController : Controller
    {
        private readonly QuizDbContext db = new QuizDbContext();
        // GET: QuizReview
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var result = db.QuizResults.FirstOrDefault(x => x.ID == id);
            if (result != null)
            {
                var vm = new QuizReviewViewModel(result);
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
            var result = db.Questions.FirstOrDefault(x => x.ID == questionID);
            if (result != null)
            {
                var vm = new QuestionReviewViewModel(result,resultId);
                return View(vm);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }
    }
}