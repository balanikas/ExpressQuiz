using System.Collections.Generic;
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
        private readonly IQuizResultRepo _quizQuizResultRepo;

        private readonly IQuizRepo _quizRepo;


        public ActiveQuizController()
        {
            _quizQuizResultRepo = new QuizResultRepo(new QuizDbContext());
            _quizRepo = new QuizRepo(new QuizDbContext());
        }
        
        // GET: ActiveQuiz
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quiz quiz = _quizRepo.GetById(id.Value);
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

            Quiz quiz = _quizRepo.GetById(id.Value);
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
            _quizQuizResultRepo.Insert(result);
            _quizQuizResultRepo.Save();
            
            return Json(result.ID);
          
        }
     
    }
}