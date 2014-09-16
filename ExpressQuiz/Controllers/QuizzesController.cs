using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Services;
using ExpressQuiz.Extensions;
using ExpressQuiz.ViewModels;

namespace ExpressQuiz.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    public class QuizzesController : Controller
    {
        private readonly IAnswerService _answerService;
        private readonly ModelConverter _modelConverter;
        private readonly IQuestionService _questionService;
        private readonly IQuizCategoryService _quizCategoryService;
        private readonly IQuizService _quizService;
        private readonly IUserActivityService _userActivityService;
        private readonly ViewModelConverter _viewModelConverter;

        public QuizzesController(IAnswerService answerService,
            IQuestionService questionService,
            IQuizCategoryService quizCategoryService,
            IQuizService quizService,
            IUserActivityService userActivityService,
            ModelConverter modelConverter,
            ViewModelConverter viewModelConverter)
        {
            _questionService = questionService;
            _answerService = answerService;
            _quizCategoryService = quizCategoryService;
            _quizService = quizService;
            _userActivityService = userActivityService;
            _modelConverter = modelConverter;
            _viewModelConverter = viewModelConverter;
        }

        private IQueryable<Quiz> GetQuizzes(string searchTerm, int? filter, int? selectedCategoryId, int? page = null)
        {
            var quizzes = _quizService.GetPublicQuizzes();

            if (filter.HasValue)
            {
                quizzes = _quizService.GetBy((QuizFilter) filter, quizzes);
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
                quizzes = quizzes.Skip(pageSize*page.Value).Take(pageSize);
            }


            return quizzes;
        }


        public ActionResult GetQuizList(string searchTerm, int? filter, int? selectedCategory, int page)
        {
            var quizzes = GetQuizzes(searchTerm, filter, selectedCategory, page);

            var vm = _modelConverter.ToQuizViewModels(quizzes);

            return PartialView("_QuizListPartial", vm);
        }

        public ActionResult Index(string searchTerm, int? filter, int? selectedCategoryId)
        {
            var quizzes = GetQuizzes(searchTerm, filter, selectedCategoryId);

            var vm = _modelConverter.ToQuizzesViewModel(quizzes, selectedCategoryId);

            return View("Index", vm);
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

            var vm = _modelConverter.ToQuizDetailsViewModel(quiz);

            var userId = String.IsNullOrEmpty(User.Identity.Name) ? Session.SessionID : User.Identity.Name;
            _userActivityService.Add(userId, ActivityItem.Quiz, ActivityAction.View, quiz.Id);

            return View("Details", vm);
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

            var vm = _modelConverter.ToEditQuizViewModel(quiz);

            return View("Edit", vm);
        }


        [HttpPost]
        [Authorize]
        public ActionResult Edit(EditQuizViewModel vm)
        {
            if (ModelState.IsValid)
            {
                Quiz quiz;
                if (_quizService.GetAll().Any(x => x.Name == vm.Quiz.Name && x.Id != vm.Quiz.QuizId))
                {
                    ModelState.AddModelError("Name", "Name already exists");
                }
                if (!String.IsNullOrEmpty(vm.NewCategory))
                {
                    if (_quizCategoryService.Exists(vm.NewCategory))
                    {
                        ModelState.AddModelError("NewCategory", "Category already exists");
                    }
                    vm.SelectedCategory = _quizCategoryService.InsertByName(vm.NewCategory).Id;
                }

                if (!ModelState.IsValid)
                {
                    quiz = _quizService.Get(vm.Quiz.QuizId);
                    vm = _modelConverter.ToEditQuizViewModel(quiz);
                    return PartialView("_EditQuizPartial", vm);
                }

                quiz = _viewModelConverter.ToQuizModel(vm);

                _quizService.Update(quiz);
                _questionService.SaveOrder(quiz, vm.QuestionOrder);

                ModelState.Clear();

                vm = _modelConverter.ToEditQuizViewModel(_quizService.Get(quiz.Id));


                _userActivityService.Add(User.Identity.Name, ActivityItem.Quiz, ActivityAction.Edit, quiz.Id);

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


            var vm = _modelConverter.ToCreateQuizViewModel(quiz, User.Identity.Name);
            return View("Create", vm);
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
                    return View("Create", vm);
                }

                if (!String.IsNullOrEmpty(vm.NewCategory))
                {
                    if (_quizCategoryService.Exists(vm.NewCategory))
                    {
                        ModelState.AddModelError("NewCategory", "Category already exists");

                        return View("Create", vm);
                    }
                }

                var quiz = _viewModelConverter.ToQuizModel(vm);
                quiz.CreatedBy = User.Identity.Name;

                _quizService.Insert(quiz);

                _userActivityService.Add(User.Identity.Name, ActivityItem.Quiz, ActivityAction.Create, quiz.Id);

                return RedirectToAction("Edit", new {id = quiz.Id});
            }

            return View("Create", vm);
        }


        [Authorize]
        public ActionResult CreateQuestion(int? id)
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

            int orderId = 0;
            if (quiz.Questions.Count > 0)
            {
                orderId = quiz.Questions.Max(x => x.Id) + 1;
            }

            var model = new Question();
            model.QuizId = id.Value;
            model.SetDefaultValues();
            model.OrderId = orderId;

            var question = _questionService.Insert(model);

            _userActivityService.Add(User.Identity.Name, ActivityItem.Question, ActivityAction.Create, question.Id);

            var vm = _modelConverter.ToEditQuizViewModel(quiz);
            return PartialView("_EditQuizPartial", vm);
        }

        [Authorize]
        public ActionResult CreateAnswer(int? questionId)
        {
            if (questionId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var question = _questionService.Get(questionId.Value);
            if (question == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            int orderId = 0;
            if (question.Answers.Count > 0)
            {
                orderId = question.Answers.Max(x => x.Id) + 1;
            }

            var model = new Answer();
            model.SetDefaultValues();
            model.QuestionId = questionId.Value;
            model.OrderId = orderId;

            var answer = _answerService.Insert(model);

            _userActivityService.Add(User.Identity.Name, ActivityItem.Answer, ActivityAction.Create, answer.Id);

            return RedirectToAction("EditQuestion", new {id = question.Id});
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

            var vm = _modelConverter.ToEditQuestionViewModel(question);
            return PartialView("_EditQuestionPartial", vm);
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditQuestion(EditQuestionViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var question = _viewModelConverter.ToQuestionModel(vm);
                _questionService.Update(question);
                _answerService.SaveOrder(question, vm.Order);

                ModelState.Clear();

                vm = _modelConverter.ToEditQuestionViewModel(question);

                _userActivityService.Add(User.Identity.Name, ActivityItem.Question, ActivityAction.Edit, question.Id);

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
            var vm = _modelConverter.ToEditAnswerViewModel(model);
            return PartialView("_EditAnswerPartial", vm);
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditAnswer(EditAnswerViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var a = _viewModelConverter.ToAnswerModel(vm);
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


            var vm = _modelConverter.ToQuizViewModel(quiz);
            return View("Delete", vm);
        }


        [HttpPost, ActionName("Delete")]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            var quiz = _quizService.Get(id);
            if (quiz.CreatedBy != User.Identity.Name)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            _quizService.Delete(id);

            _userActivityService.Add(User.Identity.Name, ActivityItem.Quiz, ActivityAction.Delete, id);


            if (Request.IsAjaxRequest())
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            return RedirectToAction("Index");
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
            var vm = _modelConverter.ToEditQuizViewModel(quiz);

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

            var vm = _modelConverter.ToEditQuestionViewModel(question);

            _userActivityService.Add(User.Identity.Name, ActivityItem.Answer, ActivityAction.Delete, id.Value);

            return PartialView("_EditQuestionPartial", vm);
        }
    }
}