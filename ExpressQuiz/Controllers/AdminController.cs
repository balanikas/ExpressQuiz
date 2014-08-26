using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Repos;
using ExpressQuiz.Core.Services;
using ExpressQuiz.Migrations;

namespace ExpressQuiz.Controllers
{

    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class AdminController : Controller
    {
        private readonly IQuizService _quizService;

        public AdminController(IQuizService quizService)
        {
            _quizService = quizService;
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
        public ActionResult Import()
        {

            var quizzes = DataProvider.Import( System.Web.HttpContext.Current.Server.MapPath("~/bin/App_Data/seeddata.xml"));

            try
            {
                foreach (var quiz in quizzes)
                {
                    if (!_quizService.GetAll().Any(x => x.Name == quiz.Name))
                    {
                        _quizService.Insert(quiz);
                    }
                }
            }
            catch (DbEntityValidationException e)
            {

                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
           
            
            return View("Index");
        }
    }
}