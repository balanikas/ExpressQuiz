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

namespace ExpressQuiz.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly IRepo<Quiz> _quizRepo;

        public UserProfileController()
        {
            var ctx = new QuizDbContext();
            _quizRepo = new Repo<Quiz>(ctx);
        }
    
        public ActionResult Index()
        {
            var quizzes = _quizRepo.GetAll().Where(x => x.CreatedBy == User.Identity.Name);


            return View(quizzes.ToList());
        }


    }
}
