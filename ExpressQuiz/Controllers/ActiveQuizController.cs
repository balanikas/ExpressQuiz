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
        private readonly IRepo<QuizResult> _quizResultRepo;

        private readonly IRepo<Quiz> _quizRepo;
        private readonly IRepo<Answer> _answerRepo;


        public ActiveQuizController()
        {
            var ctx = new QuizDbContext();
            _quizResultRepo = new Repo<QuizResult>(ctx);
            _quizRepo = new Repo<Quiz>(ctx);
            _answerRepo = new Repo<Answer>(ctx);
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
            vm.EstimatedTime = quiz.Questions.Sum(x => x.EstimatedTime);
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
            result.Score = CalculateScore(result);
            _quizResultRepo.Insert(result);
            _quizResultRepo.Save();
            
            return Json(result.Id);
          
        }

        private int CalculateScore(QuizResult result)
        {

            int count = 0;
            foreach (var userAnswer in result.Answers)
            {
                var correctAnswer = _answerRepo.GetAll().FirstOrDefault(x => x.Id == userAnswer.AnswerId);
                if (correctAnswer != null && correctAnswer.IsCorrect)
                {
                    count++;
                }

            }
            return (int) (((double) count/(double) result.Answers.Count)*100);
        }
     
    }
}