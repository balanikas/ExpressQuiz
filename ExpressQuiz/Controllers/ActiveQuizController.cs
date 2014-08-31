using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Repos;
using ExpressQuiz.Core.Services;
using ExpressQuiz.Extensions;
using ExpressQuiz.Logic;

using ExpressQuiz.ViewModels;
using Newtonsoft.Json;

namespace ExpressQuiz.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    public class ActiveQuizController : Controller
    {
        private readonly IQuizService _quizService;
        private readonly IQuizResultService _quizResultService;
        private readonly IQuestionService _questionService;
        private readonly IAnswerService _answerService;


        public ActiveQuizController(
            IQuizService quizService,
            IQuizResultService quizResultService,
            IAnswerService answerService,
            IQuestionService questionService 
            )
        {
            _quizService = quizService;
            _quizResultService = quizResultService;
            _answerService = answerService;
            _questionService = questionService;
        }
        

        public ActionResult Index(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quiz quiz = _quizService.Get(id.Value);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            var vm = quiz.ToViewModel();
            return View("Index",vm);

        }

        [HttpPost]
        public ActionResult Index(ActiveQuizViewModel vm)
        {

            return View("Index",vm);
        }

        public ActionResult GetQuiz(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Quiz quiz = _quizService.Get(id.Value);
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
            _quizResultService.Insert(result);
            
            return Json(result.Id);
          
        }


        private int CalculateScore(QuizResult result)
        {
            var quiz = _quizService.Get(result.QuizId);
            var usePoints = quiz.AllowPoints;



            int count = 0;
            var totalPoints = 0;

            if (usePoints)
            {
                
                foreach (var userAnswer in result.Answers)
                {
                    var points = _questionService.Get(userAnswer.QuestionId).Points;
                    totalPoints += points;
                    
                    var correctAnswer = _answerService.Get(userAnswer.AnswerId);
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
                    var correctAnswer = _answerService.Get(userAnswer.AnswerId);
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