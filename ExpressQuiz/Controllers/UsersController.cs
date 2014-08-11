using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpressQuiz.Models;
using ExpressQuiz.Repos;
using ExpressQuiz.ViewModels;

namespace ExpressQuiz.Controllers
{
    public class UsersController : Controller
    {
        private IRepo<QuizResult> _quizResultRepo;

        public UsersController()
        {
            var ctx = new QuizDbContext();

            _quizResultRepo = new Repo<QuizResult>(ctx);
        }
        //public ActionResult Index()
        //{
          
        //    var users = new ApplicationDbContext().Users.Select(x => new UserViewModel
        //    {
        //        Name = x.NickName
        //    });
        //    return View(users.ToList());
           
            
        //}

        public ActionResult Index(int quizId)
        {
           
                var results =_quizResultRepo.GetAll().Where(x => x.QuizId == quizId);
                
                return View("_QuizUsersPartial", results.ToList());
              
            
        }
    }
}