using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExpressQuiz.Models;
using ExpressQuiz.Repos;
using ExpressQuiz.ViewModels;
using System.IO;
using System.Web.Script.Serialization;

namespace ExpressQuiz.Controllers
{
    public class QuizzesController : Controller
    {

        private readonly IQuizCategoryRepo _quizCategoryRepo;

        private readonly IQuizRepo _quizRepo;

        public QuizzesController()
        {
            _quizRepo = new QuizRepo(new QuizDbContext());
            _quizCategoryRepo = new QuizCategoryRepo(new QuizDbContext());
        }

        // GET: Quizzes
        public ActionResult Index(int? catId, string searchString)
        {
            //var GenreLst = new List<string>();

            //var GenreQry = from d in _quizRepo.GetAll()
            //               orderby d.Summary
            //               select d.Summary;

            //GenreLst.AddRange(GenreQry.Distinct());
            //ViewBag.movieGenre = new SelectList(GenreLst);

            var quizzes = from m in _quizRepo.GetAll()
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                quizzes = quizzes.Where(s => s.Name.Contains(searchString));
            }

            if (catId.HasValue)
            {
                quizzes = quizzes.Where(x => x.Category.Id == catId);
            }

            var vm = new QuizzesViewModel();
            vm.QuizCategories = (from c in _quizCategoryRepo.GetAll() orderby c.Name select c).ToList();

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
            Quiz quiz = _quizRepo.GetById(id.Value);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            
            return View(quiz);
        }

        // GET: Quizzes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Quizzes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Summary")] Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                _quizRepo.Insert(quiz);
                _quizRepo.Save();
                return RedirectToAction("Index");
            }

            return View(quiz);
        }

        // GET: Quizzes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quiz quiz = _quizRepo.GetById(id.Value);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            return View(quiz);
        }

        // POST: Quizzes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Summary")] Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                _quizRepo.SetModified(quiz);
                _quizRepo.Save();
               
                return RedirectToAction("Index");
            }
            return View(quiz);
        }

        // GET: Quizzes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quiz quiz = _quizRepo.GetById(id.Value);
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
            Quiz quiz = _quizRepo.GetById(id);
            _quizRepo.Delete(quiz);
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


     
    }
}
