using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Repos;
using ExpressQuiz.Core.Services;
using ExpressQuiz.Extensions;
using ExpressQuiz.ViewModels;

namespace ExpressQuiz.Controllers
{

    public class QuizzesController : Controller
    {
        private readonly IAnswerService _answerService;
        private readonly IQuestionService _questionService;
        private readonly IQuizCategoryService _quizCategoryService;


        private readonly IRepo<QuizRating> _quizRatingRepo;
        private readonly IRepo<QuizResult> _quizResultRepo;
        private readonly IQuizService _quizService;

        public QuizzesController(
            IAnswerService answerService, 
            IQuestionService questionService,
            IQuizCategoryService quizCategoryService, 

            IRepo<QuizRating> quizRatingRepo, 
            IRepo<QuizResult> quizResultRepo, 
            IQuizService quizService)
        {
            _questionService = questionService;
            _answerService = answerService;
            _quizCategoryService = quizCategoryService;
            _quizRatingRepo = quizRatingRepo;
            _quizResultRepo = quizResultRepo;
            _quizService = quizService;
        }

        public ActionResult GetQuizzes(string searchTerm, int? filter, int? selectedCategory)
        {

            var quizzes = _quizService.GetPublicQuizzes();

            if (filter.HasValue)
            {
                quizzes = _quizService.GetBy((QuizFilter)filter, quizzes);
            }

            if (selectedCategory.HasValue && selectedCategory.Value != -1)
            {
                quizzes = _quizService.GetByCategory(selectedCategory.Value, quizzes);
            }

            if (!String.IsNullOrEmpty(searchTerm))
            {
                quizzes = _quizService.GetBySearchTerm(searchTerm, quizzes);
            }

            return PartialView("_QuizListPartial", quizzes.ToList());
        }

   
        public ActionResult Index(int? catId, string searchString)
        {
            var quizzes = _quizService.GetPublicQuizzes();
            if (!String.IsNullOrEmpty(searchString))
            {
                quizzes = _quizService.GetBySearchTerm(searchString, quizzes);
            }

            if (catId.HasValue && catId != -1)
            {
                quizzes = _quizService.GetByCategory(catId.Value, quizzes);
            }

            quizzes = _quizService.GetBy(QuizFilter.Newest, quizzes, true);
            var model = quizzes.ToViewModel(_quizService,_quizCategoryService,  catId);
            

            return View(model);
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

            var model = quiz.ToViewModel(_quizService, _quizResultRepo, _quizRatingRepo);

            return View(model);
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

            var vm = quiz.ToViewModel(_quizCategoryService);

            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(EditQuizViewModel model)
        {
            if (ModelState.IsValid)
            {

               

                var quiz = _quizService.Get(model.Quiz.Id);

                if (!String.IsNullOrEmpty(model.NewCategory))
                {
                    quiz.Category = _quizCategoryService.InsertByName(model.NewCategory);
                    quiz.Category.Id = quiz.Category.Id;
                }
                else
                {
                    quiz.Category = _quizCategoryService.Get(model.SelectedCategory);
                    quiz.Category.Id = model.SelectedCategory;
                }



                quiz.Summary = model.Quiz.Summary;
                quiz.Name = model.Quiz.Name;
                quiz.IsTimeable = model.Quiz.IsTimeable;
                quiz.Locked = model.Quiz.Locked;
                quiz.AllowPoints = model.Quiz.AllowPoints;

                _quizService.Update(quiz);
                
                if (quiz.Questions.Count > 1)
                {
                    _questionService.SaveOrder(quiz.Questions.ToList(), model.Order);
                }

                ModelState.Clear();

                var vm = _quizService.Get(quiz.Id).ToViewModel(_quizCategoryService);
                vm.ModifiedByUser = true;
                return PartialView("_EditQuizPartial", vm);

            }
            
            return PartialView("_EditQuizPartial", model);
        }

        // GET: Quizzes/Create
        [Authorize]
        public ActionResult Create()
        {
            var quiz = new Quiz();
            var vm = quiz.ToViewModel(_quizCategoryService, User.Identity.Name);
            return View(vm);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateQuizViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (_quizService.GetAll().Any(x => x.Name == model.Quiz.Name))
                {
                    ModelState.AddModelError("Name", "Name already exists");
                    var quiz = new Quiz();
                    var vm = quiz.ToViewModel(_quizCategoryService, User.Identity.Name);
                    return View(vm);
                }

                if (!String.IsNullOrEmpty(model.NewCategory))
                {
                    model.Quiz.Category = _quizCategoryService.InsertByName(model.NewCategory);
                }
                else
                {
                    model.Quiz.Category = _quizCategoryService.Get(model.SelectedCategory);
                }

                model.Quiz.Locked = true;

                model.Quiz.Created = DateTime.Now;

                _quizService.Insert(model.Quiz);
       
                return RedirectToAction("Edit",new {id= model.Quiz.Id});
            }

            return View(model);
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
            model.Answers = new List<Answer>();
            model.QuizId = id.Value;
            model.Text = "enter text here";
            model.EstimatedTime = 10;

            int maxOrderId = 0;
            if (quiz.Questions.Count > 0)
            {
                maxOrderId = quiz.Questions.Max(x => x.Id) + 1;
            }

            model.OrderId = maxOrderId;

            _questionService.Insert(model);
          



            var vm = quiz.ToViewModel(_quizCategoryService);
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
            model.Text = "enter text here";
            model.QuestionId = id.Value;

            int maxOrderId = 0;
            if (q.Answers.Count > 0)
            {
                maxOrderId = q.Answers.Max(x => x.Id) + 1;
            }

            model.OrderId = maxOrderId;

            _answerService.Insert(model);
      

            var vm = q.ToViewModel();

            return PartialView("_EditQuestionPartial", vm);
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

            var vm = question.ToViewModel();
            return PartialView("_EditQuestionPartial", vm);

        }

        [HttpPost]
        [Authorize]
        public ActionResult EditQuestion(EditQuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var q = _questionService.Get(model.Question.Id);
                q.Text = model.Question.Text;
                q.EstimatedTime = model.Question.EstimatedTime;
                q.Points = model.Question.Points;
                _questionService.Update(q);
          


                if (q.Answers.Count > 1)
                {
                    _answerService.SaveOrder(q.Answers.ToList(), model.Order);
                }

                ModelState.Clear();

                model = _questionService.Get(q.Id).ToViewModel();
                model.ModifiedByUser = true;
                return PartialView("_EditQuestionPartial", model);
            }
          
            return PartialView("_EditQuestionPartial", model);
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

            return PartialView("_EditAnswerPartial", model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditAnswer(Answer model)
        {
            if (ModelState.IsValid)
            {
                var a = _answerService.Get(model.Id);
                a.Explanation = model.Explanation;
                a.IsCorrect = model.IsCorrect;
                a.Text = model.Text;
                _answerService.Update(a);


                //a.Question.Answers = a.Question.Answers.OrderBy(x => x.Id).ToList();
                var vm = _questionService.Get(a.QuestionId).ToViewModel();
                vm.ModifiedByUser = true;
                ModelState.Clear();
                return PartialView("_EditQuestionPartial", vm);
            }
            return PartialView("_EditAnswerPartial", model);
        }




        // GET: Quizzes/Delete/5
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
            return View(quiz);
        }

        // POST: Quizzes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            _quizService.Delete(id);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // _quizRepo.Dispose();
            }
            base.Dispose(disposing);
        }

        [Authorize]
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
            var vm = quiz.ToViewModel(_quizCategoryService);

            return PartialView("_EditQuizPartial", vm);
        }

        [Authorize]
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

            var vm = question.ToViewModel();
            return PartialView("_EditQuestionPartial", vm);
        }



    }
}