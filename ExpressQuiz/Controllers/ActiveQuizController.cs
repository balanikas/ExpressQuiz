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
        private readonly IUserActivityService _userActivityService;

        public ActiveQuizController(IQuizService quizService, IQuizResultService quizResultService, IAnswerService answerService, IQuestionService questionService, IUserActivityService userActivityService)
        {
            _quizService = quizService;
            _quizResultService = quizResultService;
            _answerService = answerService;
            _questionService = questionService;
            _userActivityService = userActivityService;
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
            var vm = quiz.ToActiveQuizViewModel();

            var userId = String.IsNullOrEmpty(User.Identity.Name) ? Session.SessionID : User.Identity.Name;
            _userActivityService.Add(userId, ActivityItem.Quiz, ActivityAction.BeginQuiz, id.Value);

            return View("Index",vm);

        }

        //[HttpPost]
        //public ActionResult Index(ActiveQuizViewModel vm)
        //{

        //    return View("Index",vm);
        //}

        public ActionResult GetQuiz(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var quiz = _quizService.Get(id.Value);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            var vm = quiz.ToQuizViewModel(_questionService, _answerService, true, true,true,true);
          

            var jsonNetResult = new JsonNetResult();
            jsonNetResult.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
            jsonNetResult.Formatting = Formatting.Indented;
            jsonNetResult.Data = vm;

            return jsonNetResult;
        }

        
        [HttpPost]
        public JsonResult PostResult(QuizResultViewModel data)
        {

            var result = data.ToModel();

            var userId = String.IsNullOrEmpty(User.Identity.Name) ? Session.SessionID : User.Identity.Name;
            result.UserId = userId;
            result.Score = CalculateScore(result);
            _quizResultService.Insert(result);

            
            _userActivityService.Add(userId, ActivityItem.Quiz, ActivityAction.EndQuiz, result.QuizId);

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
                
                foreach (var userAnswer in result.UserAnswers)
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
                foreach (var userAnswer in result.UserAnswers)
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