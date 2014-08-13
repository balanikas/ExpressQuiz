using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ExpressQuiz.Migrations;
using ExpressQuiz.Models;
using ExpressQuiz.Repos;
using ExpressQuiz.ViewModels;
using System.IO;
using System.Web.Script.Serialization;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;

namespace ExpressQuiz.Controllers
{
     [HandleError]
    public class QuizzesController : Controller
    {
        private readonly IRepo<Answer> _answerRepo;
        private readonly IRepo<Question> _questionRepo;
        private readonly IRepo<QuizCategory> _quizCategoryRepo;

        private readonly IRepo<Quiz> _quizRepo;
        private readonly IRepo<QuizRating> _quizRatingRepo;
        private readonly IRepo<QuizResult> _quizResultRepo;

        public QuizzesController(
            IRepo<Answer> answerRepo,
            IRepo<Question> questionRepo,
            IRepo<QuizCategory> quizCategoryRepo,
            IRepo<Quiz> quizRepo,
            IRepo<QuizRating> quizRatingRepo,
            IRepo<QuizResult> quizResultRepo
            )
        {
            _quizRepo = quizRepo;
            _questionRepo = questionRepo;
            _answerRepo = answerRepo;
            _quizCategoryRepo = quizCategoryRepo;
            _quizRatingRepo = quizRatingRepo;
            _quizResultRepo = quizResultRepo;

        }

        public ActionResult GetQuizzes(string searchTerm, int? filter, int? selectedCategory)
        {
            IEnumerable<Quiz> quizzes = from m in _quizRepo.GetAll()
                                               select m;

            if (filter.HasValue)
            {
                quizzes = _quizRepo.AsOrdered(_quizRatingRepo, (QuizFilter) filter);
            }


            if (selectedCategory.HasValue && selectedCategory.Value != -1)
            {
                quizzes = quizzes.Where(s => s.QuizCategoryId == selectedCategory.Value);
            }

            if (!String.IsNullOrEmpty(searchTerm))
            {
                quizzes = quizzes.Where(s => s.Name.IndexOf(searchTerm,StringComparison.OrdinalIgnoreCase) != -1);
            }

            return PartialView("_QuizListPartial",quizzes.ToList());
        }

        // GET: Quizzes
        public ActionResult Index(int? catId, string searchString)
        {
            
            
            var quizzes = from m in _quizRepo.GetAll()
                select m;

            var model = quizzes.ToViewModel(_quizCategoryRepo, _quizRatingRepo, _quizRepo, catId, searchString);

            return View(model);
        }


       
        public ActionResult Details(int? id)
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

            var model = quiz.ToViewModel(_quizResultRepo, _quizRatingRepo);

            return View(model);
        }


        //private IEnumerable<SelectListItem> GetFilters()
        //{
        //    var filters = new List<SelectListItem>();
        //    foreach (QuizFilter f in (QuizFilter[])Enum.GetValues(typeof(QuizFilter)))
        //    {
        //        filters.Add(new SelectListItem()
        //        {
        //            Value = Convert.ChangeType(f, f.GetTypeCode()).ToString(),
        //            Text = f.ToString()
        //        });
        //    }

            

        //    return new SelectList(filters, "Value", "Text");
        //}
      

  
         [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quiz quiz = _quizRepo.Get(id.Value);

             if (quiz.Locked)
             {
                 if (quiz.CreatedBy != User.Identity.Name)
                 {
                     return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                 }
             }

             var vm = quiz.ToViewModel(_quizCategoryRepo);
           
            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(EditQuizViewModel model)
        {
            if (ModelState.IsValid)
            {

                var quiz = _quizRepo.Get(model.Quiz.Id);

                if (!String.IsNullOrEmpty(model.NewCategory))
                {
                    quiz.Category = _quizCategoryRepo.Insert(model.NewCategory);
                    quiz.QuizCategoryId = quiz.Category.Id;
                }
                else
                {
                    quiz.Category = _quizCategoryRepo.Get(model.SelectedCategory);
                    quiz.QuizCategoryId = model.SelectedCategory;
                }


               
                quiz.Summary = model.Quiz.Summary;
                quiz.Name = model.Quiz.Name;
                quiz.IsTimeable = model.Quiz.IsTimeable;
                quiz.Locked = model.Quiz.Locked;

                _quizRepo.Update(quiz);
                _quizRepo.Save();
                if (quiz.Questions.Count > 1)
                {
                   _questionRepo.SaveOrder(quiz,model.Order);
                }

                model = quiz.ToViewModel(_quizCategoryRepo);

                return PartialView("_EditQuizPartial", model);
                
            }
            return View(model);
        }

        // GET: Quizzes/Create
        [Authorize]
        public ActionResult Create()
        {
            
            var vm = new CreateQuizViewModel();
            vm.Categories = _quizCategoryRepo.GetCategoriesAsSelectList();
            vm.Quiz = new Quiz();
            vm.Quiz.CreatedBy = User.Identity.Name;
            return View(vm);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateQuizViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!String.IsNullOrEmpty(model.NewCategory))
                {
                    model.Quiz.Category = _quizCategoryRepo.Insert(model.NewCategory);
                }
                else
                {
                    model.Quiz.Category = _quizCategoryRepo.Get(model.SelectedCategory);
                }

              
                model.Quiz.Created = DateTime.Now;
                
                _quizRepo.Insert(model.Quiz);
                _quizRepo.Save();
                return RedirectToAction("Index");
            }

            return View(model);
        }


         [Authorize]
        public ActionResult CreateQuestion(int id, int orderId)
        {
            var model = new Question();
            model.Answers = new List<Answer>();
            model.QuizId = id;
            model.Text = "enter text here";

            var quiz = _quizRepo.Get(id);
            int maxOrderId = 0;
            if (quiz.Questions.Count > 0)
            {
                maxOrderId  = quiz.Questions.Max(x => x.Id) + 1;
            }
          
            model.OrderId = maxOrderId;

            _questionRepo.Insert(model);
            _questionRepo.Save();



             var vm = quiz.ToViewModel(_quizCategoryRepo);
            return PartialView("_EditQuizPartial", vm);
        }

         [Authorize]
        public ActionResult CreateAnswer(int id, int orderId)
        {
            var model = new Answer();
            model.Text = "enter text here";
            model.QuestionId = id;

            var q = _questionRepo.Get(id);
            int maxOrderId = 0;
            if (q.Answers.Count > 0)
            {
                maxOrderId = q.Answers.Max(x => x.Id) + 1;
            }

            model.OrderId = maxOrderId;

            _answerRepo.Insert(model);
            _answerRepo.Save();

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
          
            var question = _questionRepo.Get(id.Value);


             var vm = question.ToViewModel();
            return PartialView("_EditQuestionPartial", vm);

        }

        [HttpPost]
        [Authorize]
        public ActionResult EditQuestion(EditQuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var q = _questionRepo.Get(model.Question.Id);
                q.Text = model.Question.Text;
                q.EstimatedTime = model.Question.EstimatedTime;
                _questionRepo.Update(q);
                _questionRepo.Save();


                if (q.Answers.Count > 1)
                {
                    _answerRepo.SaveOrder(q,model.Order);
                }
                
                ModelState.Clear();

                model = q.ToViewModel();

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
            if (Request.IsAjaxRequest())
            {
                var model = _answerRepo.Get(id.Value);


                return PartialView("_EditAnswerPartial", model);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotAcceptable);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditAnswer(Answer answer)
        {
            if (ModelState.IsValid)
            {
                var a = _answerRepo.Get(answer.Id);
                a.Explanation = answer.Explanation;
                a.IsCorrect = answer.IsCorrect;
                a.Text = answer.Text;
                _answerRepo.Update(a);
                _answerRepo.Save();

                a.Question.Answers = a.Question.Answers.OrderBy(x => x.Id).ToList();
                var vm = a.Question.ToViewModel();

                ModelState.Clear();
                return PartialView("_EditQuestionPartial",vm);
            }
            return PartialView("_EditAnswerPartial", answer);
        }

      


        // GET: Quizzes/Delete/5
         [Authorize]
        public ActionResult Delete(int? id)
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
            return View(quiz);
        }

        // POST: Quizzes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            _quizRepo.Delete(id);
            _quizRepo.Save();
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
            var model = _questionRepo.Get(id.Value);
            var quiz = model.Quiz;

            _questionRepo.Delete(id.Value);
            _questionRepo.Save();

            var vm = model.Quiz.ToViewModel(_quizCategoryRepo);

            return PartialView("_EditQuizPartial", vm);
        }

         [Authorize]
        public ActionResult DeleteAnswer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = _answerRepo.Get(id.Value);
            var question = model.Question;

            _answerRepo.Delete(id.Value);
            _answerRepo.Save();

             var vm = question.ToViewModel();
            return PartialView("_EditQuestionPartial", vm);
        }


      
    }
}