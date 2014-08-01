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

namespace ExpressQuiz.Controllers
{
    public class QuizzesController : Controller
    {
        private readonly IRepo<Answer> _answerRepo;
        private readonly IRepo<Question> _questionRepo;
        private readonly IRepo<QuizCategory> _quizCategoryRepo;

        private readonly IRepo<Quiz> _quizRepo;

        public QuizzesController()
        {
            var ctx = new QuizDbContext();
           
            _quizRepo = new Repo<Quiz>(ctx);
            _questionRepo = new Repo<Question>(ctx);
            _answerRepo = new Repo<Answer>(ctx);
            _quizCategoryRepo = new Repo<QuizCategory>(ctx);
           
        }

        // GET: Quizzes
        public ActionResult Index(int? catId, string searchString)
        {
            var quizzes = from m in _quizRepo.GetAll()
                select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                quizzes = quizzes.Where(s => s.Name.Contains(searchString));
            }

            if (catId.HasValue && catId != -1)
            {
                quizzes = quizzes.Where(x => x.Category.Id == catId);
            }

            var vm = new QuizzesViewModel();
            vm.QuizCategories = (from c in _quizCategoryRepo.GetAll() orderby c.Name select c).ToList();
            vm.QuizCategories.Insert(0,new QuizCategory(){Id = -1, Name ="All"});
            vm.Quizzes = quizzes.ToList();

            vm.TopQuizzes = (from t in _quizRepo.GetAll() orderby t.Name select t).ToList();
            return View(vm);
        }




        // GET: Quizzes/Details/5
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

            return View(quiz);
        }

        private IEnumerable<SelectListItem> GetCategories()
        {
            var cats = _quizCategoryRepo.GetAll()
                .Select(x =>
                    new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name
                    });


            return new SelectList(cats, "Value", "Text");
        }

        // GET: Quizzes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quiz quiz = _quizRepo.Get(id.Value);

            var vm = new EditQuizViewModel();
            vm.Quiz = quiz;
            var sortedQuestions = quiz.Questions.AsQueryable().AsNoTracking().OrderBy(x => x.OrderId).Select(x => x.Id);
            vm.Order = string.Join(",", sortedQuestions);
            vm.Categories = GetCategories();
            vm.SelectedCategory = quiz.Category.Id;
            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditQuizViewModel model)
        {
            if (ModelState.IsValid)
            {

                var quiz = _quizRepo.Get(model.Quiz.Id);
                quiz.QuizCategoryId = model.SelectedCategory;
                quiz.Summary = model.Quiz.Summary;
                quiz.Name = model.Quiz.Name;

                _quizRepo.Update(quiz);
                _quizRepo.Save();

                var orders = model.Order.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                int orderCount = 0;
                foreach (var order in orders)
                {
                    var q = quiz.Questions.First(x => x.Id.ToString() == order);
                    q.OrderId = orderCount++;
                    _questionRepo.Update(q);
                }
                _questionRepo.Save();

                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Quizzes/Create
        public ActionResult Create()
        {
            var vm = new CreateQuizViewModel();
            vm.Categories = GetCategories();
            vm.Quiz = new Quiz();
            return View(vm);
        }

        // POST: Quizzes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "Id,Name,Summary")]*/ CreateQuizViewModel vm)
        {
            if (ModelState.IsValid)
            {
                vm.Quiz.Category = _quizCategoryRepo.Get(vm.SelectedCategory);
                _quizRepo.Insert(vm.Quiz);
                _quizRepo.Save();
                return RedirectToAction("Index");
            }

            return View(vm);
        }

      

        public ActionResult CreateQuestion(int id, int orderId)
        {
            var model = new Question();
            model.Answers = new List<Answer>();
            model.QuizId = id;
            model.Text = "enter text here";

            var maxOrderId = _quizRepo.Get(id).Questions.Max(x => x.Id);
            model.OrderId = maxOrderId + 1;

            _questionRepo.Insert(model);
            _questionRepo.Save();

            var quiz = _quizRepo.Get(id);

            var vm = new EditQuizViewModel()
            {
                Quiz = quiz,
                Order = string.Join(",", quiz.Questions.OrderBy(x=>x.Id).Select(x => x.Id))
            };
            return PartialView("_EditQuizPartial", vm);
        }

        public ActionResult CreateAnswer(int id, int orderId)
        {
            var model = new Answer();
            model.Text = "enter text here";
            model.QuestionId = id;

            var maxOrderId = _questionRepo.Get(id).Answers.Max(x=>x.Id);
            model.OrderId = maxOrderId + 1;

            _answerRepo.Insert(model);
            _answerRepo.Save();

            var q = _questionRepo.Get(id);

            var vm = new EditQuestionViewModel()
            {
                Question = q,
                Order = string.Join(",", q.Answers.OrderBy(x=>x.Id).Select(x => x.Id))
            };
            return PartialView("_EditQuestionPartial", vm);
        }

       

        public ActionResult EditQuestion(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
          
            var question = _questionRepo.Get(id.Value);


            var vm = new EditQuestionViewModel();
            vm.Question = question;

            var sortedAnswers = question.Answers.AsQueryable().AsNoTracking().OrderBy(x => x.OrderId).Select(x => x.Id);
        
            vm.Order = string.Join(",", sortedAnswers);
            return PartialView("_EditQuestionPartial", vm);

        }

        [HttpPost]
        public ActionResult EditQuestion(EditQuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var q = _questionRepo.Get(model.Question.Id);
                q.Text = model.Question.Text;
                
                _questionRepo.Update(q);
                _questionRepo.Save();

              

                var orders = model.Order.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                int orderCount = 0;
                foreach (var order in orders)
                {
                    var a = q.Answers.First(x => x.Id.ToString() == order);
                    a.OrderId = orderCount++;
                    _answerRepo.Update(a);
                }
                _answerRepo.Save();



                var quiz = _quizRepo.Get(model.Question.QuizId);
               
                var vm = new EditQuizViewModel()
                {
                    Quiz = quiz,
                    Order = string.Join(",", quiz.Questions.OrderBy(x => x.Id).Select(x => x.Id)),
                    Categories = GetCategories(),
                    SelectedCategory = quiz.QuizCategoryId
                    
                };
                ModelState.Clear();
                return PartialView("_EditQuizPartial", vm);
            }
            return PartialView("_EditQuestionPartial", model);
        }

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
                var vm = new EditQuestionViewModel()
                {
                    Question = a.Question,
                    Order = string.Join(",", a.Question.Answers.OrderBy(x => x.Id).Select(x => x.Id))
                };
                ModelState.Clear();
                return PartialView("_EditQuestionPartial",vm);
            }
            return PartialView("_EditAnswerPartial", answer);
        }

       

        // GET: Quizzes/Delete/5
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
                _quizRepo.Dispose();
            }
            base.Dispose(disposing);
        }


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

            var vm = new EditQuizViewModel()
            {
                Quiz = quiz,
                Order = string.Join(",", quiz.Questions.Select(x => x.Id))
            };
            return PartialView("_EditQuizPartial", vm);
        }

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

            var vm = new EditQuestionViewModel()
            {
                Question = question,
                Order = string.Join(",", question.Answers.Select(x => x.Id))
            };
            return PartialView("_EditQuestionPartial", vm);
        }
    }
}