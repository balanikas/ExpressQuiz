using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ExpressQuiz.Logic;
using ExpressQuiz.Models;
using ExpressQuiz.Repos;
using ExpressQuiz.ViewModels;
using Newtonsoft.Json;

namespace ExpressQuiz.Controllers
{
    public class ActiveQuizController : Controller
    {
        private readonly IRepo<QuizResult> _quizQuizResultRepo;

        private readonly IRepo<Quiz> _quizRepo;


        public ActiveQuizController()
        {
            var ctx = new QuizDbContext();
            _quizQuizResultRepo = new Repo<QuizResult>(ctx);
            _quizRepo = new Repo<Quiz>(ctx);
        }
        
        // GET: ActiveQuiz
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quiz quiz = _quizRepo.Get(id.Value);
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

            Quiz quiz = _quizRepo.Get(id.Value);
            if (quiz == null)
            {
                return HttpNotFound();
            }

            quiz.Questions = quiz.Questions.AsQueryable().AsNoTracking().OrderBy(x => x.OrderId).ToArray();
            foreach (var question in quiz.Questions)
            {
                question.Answers = question.Answers.AsQueryable().AsNoTracking().OrderBy(x => x.OrderId).ToArray();
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
            _quizQuizResultRepo.Insert(result);
            _quizQuizResultRepo.Save();
            
            return Json(result.Id);
          
        }
     
    }
}