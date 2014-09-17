using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpressQuiz.Core.Services;
using ExpressQuiz.Core.Utils;

namespace ExpressQuiz.Controllers
{
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class AdminController : Controller
    {
        private readonly IQuizCategoryService _quizCategoryService;
        private readonly IQuizService _quizService;

        public AdminController(IQuizService quizService, IQuizCategoryService quizCategoryService)
        {
            _quizService = quizService;
            _quizCategoryService = quizCategoryService;
        }

        public ActionResult Index()
        {
            return View("Index");
        }

        public FileResult Export()
        {
            var quizzes = from m in _quizService.GetAll().OrderByDescending(x => x.Created)
                select m;
            DataProvider.Export(quizzes.ToList(),
                System.Web.HttpContext.Current.Server.MapPath("~/bin/App_Data/exported.xml"));
            return File(System.Web.HttpContext.Current.Server.MapPath("~/bin/App_Data/exported.xml"), "text/xml");
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


            int successCount = 0, failCount = 0;
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
                        successCount++;
                    }
                    catch (Exception)
                    {
                        failCount++;
                        //ignore, probably bad format, or validation issue
                    }
                }
            }
            ViewData["ImportStatus"] = string.Format("{0} quizzes imported, {1} quizzes were not imported", successCount,
                failCount);
            return RedirectToAction("Index");
        }
    }
}