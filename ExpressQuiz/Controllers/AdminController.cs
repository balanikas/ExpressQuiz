using System;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Repos;
using ExpressQuiz.Core.Services;
using ExpressQuiz.Core.Utils;


namespace ExpressQuiz.Controllers
{

    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class AdminController : Controller
    {
        private readonly IQuizService _quizService;
        private readonly IQuizCategoryService _quizCategoryService;

        public AdminController(IQuizService quizService, IQuizCategoryService quizCategoryService)
        {
            _quizService = quizService;
            _quizCategoryService = quizCategoryService;
        }
        // GET: Admin
        public ActionResult Index()
        {
            return View("Index");
        }

        public FileResult Export()
        {

            var quizzes = from m in _quizService.GetAll().OrderByDescending(x => x.Created)
                          select m;
            DataProvider.Export(quizzes.ToList(), System.Web.HttpContext.Current.Server.MapPath("~/bin/App_Data/seeddata.xml"));
            return File(System.Web.HttpContext.Current.Server.MapPath("~/bin/App_Data/seeddata.xml"), "text/xml");
        }

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase file)
        {

            if (file.ContentLength <= 0)
            {

                return RedirectToAction("Index"); //todo: report error
            }


            var fileName = Path.GetFileName(file.FileName);
            var path = Path.Combine(Server.MapPath("~/App_Data"), fileName);
            file.SaveAs(path);


            var quizzes = DataProvider.Import(path);
            System.IO.File.Delete(path);
            
            foreach (var quiz in quizzes)
            {
                if (!_quizService.GetAll().Any(x => x.Name == quiz.Name))
                {
                    var cat = _quizCategoryService.GetAll().FirstOrDefault(x => x.Name == quiz.Category.Name);
                    if (cat != null)
                    {
                        quiz.Category = cat;
                    }
                    try
                    {
                        _quizService.Insert(quiz);
                    }
                    catch (Exception)
                    {
                        
                        //ignore, probably bad format, or validation issue
                    }
                    
                }
            }

            return RedirectToAction("Index");
        }
    }
}