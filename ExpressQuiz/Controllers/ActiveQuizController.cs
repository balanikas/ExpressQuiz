using System;
using System.Net;
using System.Web.Mvc;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Services;
using ExpressQuiz.Logic;
using ExpressQuiz.ViewModels;
using Newtonsoft.Json;

namespace ExpressQuiz.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    public class ActiveQuizController : Controller
    {
        private readonly ModelConverter _modelConverter;
        private readonly IQuizResultService _quizResultService;
        private readonly IQuizService _quizService;
        private readonly IUserActivityService _userActivityService;
        private readonly ViewModelConverter _viewModelConverter;

        public ActiveQuizController(IQuizService quizService, IQuizResultService quizResultService,
            IUserActivityService userActivityService,
            ModelConverter modelConverter, ViewModelConverter viewModelConverter)
        {
            _quizService = quizService;
            _quizResultService = quizResultService;
            _userActivityService = userActivityService;
            _modelConverter = modelConverter;
            _viewModelConverter = viewModelConverter;
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
            var vm = _modelConverter.ToActiveQuizViewModel(quiz);

            var userId = String.IsNullOrEmpty(User.Identity.Name) ? Session.SessionID : User.Identity.Name;
            _userActivityService.Add(userId, ActivityItem.Quiz, ActivityAction.BeginQuiz, id.Value);

            return View("Index", vm);
        }


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
            var vm = _modelConverter.ToQuizViewModel(quiz, true, true, true, true);


            var jsonNetResult = new JsonNetResult();
            jsonNetResult.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
            jsonNetResult.Formatting = Formatting.Indented;
            jsonNetResult.Data = vm;

            return jsonNetResult;
        }


        [HttpPost]
        public JsonResult PostResult(QuizResultViewModel vm)
        {
            var result = _viewModelConverter.ToQuizResultModel(vm);

            var userId = String.IsNullOrEmpty(User.Identity.Name) ? Session.SessionID : User.Identity.Name;
            result.UserId = userId;

            _quizResultService.Insert(result);


            _userActivityService.Add(userId, ActivityItem.Quiz, ActivityAction.EndQuiz, result.QuizId);

            return Json(result.Id);
        }
    }
}