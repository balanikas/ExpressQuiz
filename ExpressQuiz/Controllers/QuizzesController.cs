using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Repos;
using ExpressQuiz.Core.Services;
using ExpressQuiz.Core.Utils;
using ExpressQuiz.Extensions;
using ExpressQuiz.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace ExpressQuiz.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    public class QuizzesController : Controller
    {
        private readonly IAnswerService _answerService;
        private readonly IQuestionService _questionService;
        private readonly IQuizCategoryService _quizCategoryService;
        private readonly IRepo<QuizRating> _quizRatingRepo;
        private readonly IQuizResultService _quizResultService;
        private readonly IQuizService _quizService;
        private readonly IUserActivityService _userActivityService;

        public QuizzesController(IAnswerService answerService, 
            IQuestionService questionService, 
            IQuizCategoryService quizCategoryService,
            IRepo<QuizRating> quizRatingRepo,
            IQuizResultService quizResultService, 
            IQuizService quizService, 
            IUserActivityService userActivityService)
        {
            _questionService = questionService;
            _answerService = answerService;
            _quizCategoryService = quizCategoryService;
            _quizRatingRepo = quizRatingRepo;
            _quizResultService = quizResultService;
            _quizService = quizService;
            _userActivityService = userActivityService;
        }

        private IQueryable<Quiz> GetQuizzes(string searchTerm, int? filter, int? selectedCategoryId, int? page = null)
        {

            var quizzes = _quizService.GetPublicQuizzes();

            
            if (filter.HasValue)
            {
                quizzes = _quizService.GetBy((QuizFilter)filter, quizzes);
            }
            else
            {
                quizzes = _quizService.GetBy(QuizFilter.Newest, quizzes);
            }

            if (selectedCategoryId.HasValue && selectedCategoryId.Value != -1)
            {
                quizzes = _quizService.GetByCategory(selectedCategoryId.Value, quizzes);
            }

            if (!String.IsNullOrEmpty(searchTerm))
            {
                quizzes = _quizService.GetBySearchTerm(searchTerm, quizzes);
            }

            if (page.HasValue)
            {
                const int pageSize = 10;
                page--;
                quizzes = quizzes.Skip(pageSize * page.Value).Take(pageSize);
            }
           
            

            return quizzes;
        }


        public ActionResult GetQuizList(string searchTerm, int? filter, int? selectedCategory, int page)
        {
           
            
            var quizzes = GetQuizzes(searchTerm, filter, selectedCategory,page);

            var vm = quizzes.ToQuizViewModels();

            return PartialView("_QuizListPartial", vm);
        }

        public ActionResult Index(string searchTerm, int? filter, int? selectedCategoryId)
        {

            var quizzes = GetQuizzes(searchTerm, filter, selectedCategoryId);

            var vm = quizzes.ToQuizzesViewModel(_questionService, _quizService, _quizCategoryService, selectedCategoryId);
            
            return View("Index",vm);
        }

        public ActionResult Details(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            Quiz quiz = _quizService.Get(id.Value);
            if (quiz == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var vm = quiz.ToQuizDetailsViewModel(_questionService,_answerService, _quizResultService, _quizRatingRepo);

            var userId = String.IsNullOrEmpty(User.Identity.Name) ? Session.SessionID : User.Identity.Name;
            _userActivityService.Add(userId, ActivityItem.Quiz, ActivityAction.View, quiz.Id);

            return View("Details",vm);
        }

        [Authorize]
        public ActionResult Edit(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quiz quiz = _quizService.Get(id.Value);
            if (quiz == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            if (quiz.Locked)
            {
                if (quiz.CreatedBy != User.Identity.Name)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
            }

            var vm = quiz.ToEditQuizViewModel(_questionService, _answerService,_quizCategoryService);

            return View("Edit",vm);
        }


        [HttpPost]
        [Authorize]
        public ActionResult Edit(EditQuizViewModel vm)
        {
            
            if (ModelState.IsValid)
            {
                Quiz quiz;
                if (_quizService.GetAll().Any(x=>x.Name == vm.Quiz.Name && x.Id != vm.Quiz.QuizId))
                {
                    ModelState.AddModelError("Name", "Name already exists");
                    quiz = _quizService.Get(vm.Quiz.QuizId);
                    vm = quiz.ToEditQuizViewModel(_questionService, _answerService,_quizCategoryService);
                    return PartialView("_EditQuizPartial", vm);
                }
                if (!String.IsNullOrEmpty(vm.NewCategory))
                {
                    if (_quizCategoryService.Exists(vm.NewCategory))
                    {
                        ModelState.AddModelError("NewCategory", "Category already exists");
                        quiz = _quizService.Get(vm.Quiz.QuizId);
                        vm = quiz.ToEditQuizViewModel(_questionService, _answerService,_quizCategoryService);
                        return PartialView("_EditQuizPartial", vm);
                    }
                    vm.SelectedCategory = _quizCategoryService.InsertByName(vm.NewCategory).Id;
                }



                quiz = vm.ToModel(_quizService, _quizCategoryService);

                _quizService.Update(quiz);
                
                if (quiz.Questions.Count > 1)
                {
                    _questionService.SaveOrder(quiz.Id, vm.QuestionOrder);
                }

                ModelState.Clear();

                vm = _quizService.Get(quiz.Id).ToEditQuizViewModel(_questionService, _answerService,_quizCategoryService);

          
                _userActivityService.Add(User.Identity.Name,ActivityItem.Quiz,ActivityAction.Edit, quiz.Id);

                return PartialView("_EditQuizPartial", vm);

            }
            
            return PartialView("_EditQuizPartial", vm);
        }


        [Authorize]
        public ActionResult Create()
        {
            var quiz = new Quiz();
            quiz.IsTimeable = true;
            quiz.AllowPoints = true;


            var vm = quiz.ToCreateQuizViewModel(_quizCategoryService, User.Identity.Name);
            return View("Create",vm);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(CreateQuizViewModel vm)
        {
            if (ModelState.IsValid)
            {
               
                if (_quizService.Exists(vm.Quiz.Name))
                {
                    ModelState.AddModelError("Name", "Name already exists");
                    return View("Create",vm);
                }

                if (!String.IsNullOrEmpty(vm.NewCategory))
                {
                    if (_quizCategoryService.Exists( vm.NewCategory))
                    {
                        ModelState.AddModelError("NewCategory", "Category already exists");
                    
                        return View("Create", vm);
                    }
                }

                var quiz = vm.ToModel(_quizCategoryService);
                quiz.CreatedBy = User.Identity.Name;

                _quizService.Insert(quiz);

                _userActivityService.Add(User.Identity.Name, ActivityItem.Quiz, ActivityAction.Create, quiz.Id);
             
                return RedirectToAction("Edit",new {id= quiz.Id});
            }

            return View("Create",vm);
        }


        [Authorize]
        public ActionResult CreateQuestion(int? id, int orderId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var quiz = _quizService.Get(id.Value);
            if (quiz == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var model = new Question();
        
            model.QuizId = id.Value;
            model.SetDefaultValues();

            int maxOrderId = 0;
            if (quiz.Questions.Count > 0)
            {
                maxOrderId = quiz.Questions.Max(x => x.Id) + 1;
            }

            model.OrderId = maxOrderId;

            var question = _questionService.Insert(model);

            _userActivityService.Add(User.Identity.Name, ActivityItem.Question, ActivityAction.Create, question.Id);

            var vm = quiz.ToEditQuizViewModel(_questionService, _answerService,_quizCategoryService);
            return PartialView("_EditQuizPartial", vm);
        }

        [Authorize]
        public ActionResult CreateAnswer(int? id, int orderId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var q = _questionService.Get(id.Value);
            if (q == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            
            var model = new Answer();
            model.SetDefaultValues();
            model.QuestionId = id.Value;

            int maxOrderId = 0;
            if (q.Answers.Count > 0)
            {
                maxOrderId = q.Answers.Max(x => x.Id) + 1;
            }

            model.OrderId = maxOrderId;

           var answer = _answerService.Insert(model);

           _userActivityService.Add(User.Identity.Name, ActivityItem.Answer, ActivityAction.Create, answer.Id);

            return RedirectToAction("EditQuestion", new {id = q.Id});

        }


        [Authorize]
        public ActionResult EditQuestion(int? id)
        {
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var question = _questionService.Get(id.Value);
            if (question == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var vm = question.ToEditQuestionViewModel(_answerService,_quizService);
            return PartialView("_EditQuestionPartial", vm);

        }

        [HttpPost]
        [Authorize]
        public ActionResult EditQuestion(EditQuestionViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var q = vm.ToModel(_questionService);
                _questionService.Update(q);
          
                if (q.Answers.Count > 1)
                {
                    _answerService.SaveOrder(q.Id, vm.Order);
                }

                ModelState.Clear();

                vm = _questionService.Get(q.Id).ToEditQuestionViewModel(_answerService,_quizService);

                _userActivityService.Add(User.Identity.Name, ActivityItem.Question, ActivityAction.Edit, q.Id);

                return PartialView("_EditQuestionPartial", vm);
            }
          
            return PartialView("_EditQuestionPartial", vm);
        }

        [Authorize]
        public ActionResult EditAnswer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var model = _answerService.Get(id.Value);
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            var vm = model.ToEditAnswerViewModel();
            return PartialView("_EditAnswerPartial", vm);
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditAnswer(EditAnswerViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var a = _answerService.Get(vm.Answer.AnswerId);
                a.Explanation = vm.Answer.Explanation;
                a.IsCorrect = vm.Answer.IsCorrect;
                a.Text = vm.Answer.Text;
                _answerService.Update(a);

                ModelState.Clear();

                _userActivityService.Add(User.Identity.Name, ActivityItem.Answer, ActivityAction.Edit, a.Id);

                return RedirectToAction("EditQuestion", new {id = a.QuestionId});
            }
            return PartialView("_EditAnswerPartial", vm);
        }

        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quiz quiz = _quizService.Get(id.Value);
            if (quiz == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }


            var vm = quiz.ToQuizViewModel(_questionService, _answerService);
            return View("Delete", vm);
            
        }


        [HttpPost, ActionName("Delete")]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            _quizService.Delete(id);

            _userActivityService.Add(User.Identity.Name, ActivityItem.Quiz, ActivityAction.Delete, id);


            if (Request.IsAjaxRequest())
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            else
            {
                return RedirectToAction("Index");
            }
            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
            base.Dispose(disposing);
        }

        [Authorize]
        [HttpPost]
        public ActionResult DeleteQuestion(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = _questionService.Get(id.Value);

            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            _questionService.Delete(id.Value);



            var quiz = _quizService.Get(model.QuizId);
            var vm = quiz.ToEditQuizViewModel(_questionService, _answerService,_quizCategoryService);

            _userActivityService.Add(User.Identity.Name, ActivityItem.Question, ActivityAction.Delete, id.Value);

            return PartialView("_EditQuizPartial", vm);
        }

        [Authorize]
        [HttpPost]
        public ActionResult DeleteAnswer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = _answerService.Get(id.Value);
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            _answerService.Delete(id.Value);
 

            var question = _questionService.Get(model.QuestionId);

            var vm = question.ToEditQuestionViewModel(_answerService,_quizService);

            _userActivityService.Add(User.Identity.Name, ActivityItem.Answer, ActivityAction.Delete, id.Value);

            return PartialView("_EditQuestionPartial", vm);
        }



    }
}