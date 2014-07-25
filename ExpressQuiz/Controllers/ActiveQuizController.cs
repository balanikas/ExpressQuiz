using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ExpressQuiz.Logic;
using ExpressQuiz.Models;
using ExpressQuiz.ViewModels;
using Newtonsoft.Json;

namespace ExpressQuiz.Controllers
{
    public class ActiveQuizController : Controller
    {
        private readonly QuizDbContext db = new QuizDbContext();
        // GET: ActiveQuiz
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quiz quiz = db.Quizzes.Find(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            var vm = new ActiveQuizViewModel();
            vm.Quiz = quiz;

            return View(vm);

        }

        [HttpPost]
        public ActionResult Index(ActiveQuizViewModel vm)
        {

            return View(vm);
        }

        public ActionResult GetQuiz(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
         
            Quiz quiz = db.Quizzes.Find(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }


            JsonNetResult jsonNetResult = new JsonNetResult();
            jsonNetResult.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
            jsonNetResult.Formatting = Formatting.Indented;
            jsonNetResult.Data = quiz;

            return jsonNetResult;
        }

     


        [HttpPost]
        public JsonResult PostResult(QuizResult result)
        {
            db.QuizResults.Add(result);
            db.SaveChanges();
            return Json(result.ID);
          
        }
        public ActionResult Summary(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}

            //var result = db.QuizResults.FirstOrDefault(x => x.ID == id);
            //if (result != null)
            //{
            //    var vm = new QuizReviewViewModel(result);
            //    //vm.Score = 56;
            //    return View(vm);
            //}
            
            return new HttpStatusCodeResult(HttpStatusCode.ExpectationFailed);
        }

 
    }
}