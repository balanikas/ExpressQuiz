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
        private readonly IRepo<Question> _questionRepo;
        private readonly IRepo<Quiz> _quizRepo;
        private readonly IRepo<Answer> _answerRepo;


        public ActiveQuizController(
            IRepo<QuizResult> quizResultRepo,
            IRepo<Quiz> quizRepo,
            IRepo<Answer> answerRepo,
            IRepo<Question> questionRepo 
            )
        {
            _quizResultRepo = quizResultRepo;
            _quizRepo = quizRepo;
            _answerRepo = answerRepo;
            _questionRepo = questionRepo;
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
            var vm = quiz.ToViewModel();
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
            result.UserId = User.Identity.Name;
            result.Score = CalculateScore(result);
            _quizResultRepo.Insert(result);
            _quizResultRepo.Save();
            
            return Json(result.Id);
          
        }

        private int CalculateScore(QuizResult result)
        {
            var quiz = _quizRepo.Get(result.QuizId);
            var usePoints = quiz.AllowPoints;



            int count = 0;
            var totalPoints = 0;

            if (usePoints)
            {
                
                foreach (var userAnswer in result.Answers)
                {
                    var points = _questionRepo.Get(userAnswer.QuestionId).Points;
                    totalPoints += points;
                    
                    var correctAnswer = _answerRepo.Get(userAnswer.AnswerId);
                    if (correctAnswer != null && correctAnswer.IsCorrect)
                    {
                        count += points;
                    }

                }
                return (int)(((double)count / (double)totalPoints) * 100);
                
            }
            else
            {
                totalPoints = quiz.Questions.Count;
                foreach (var userAnswer in result.Answers)
                {
                    var correctAnswer = _answerRepo.Get(userAnswer.AnswerId);
                    if (correctAnswer != null && correctAnswer.IsCorrect)
                    {
                        count++;
                    }

                }
                return (int) (((double)count / (double)totalPoints) * 100);
            }
          
           
        }
     
    }
}