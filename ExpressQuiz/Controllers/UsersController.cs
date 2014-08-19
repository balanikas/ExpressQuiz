using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Repos;
using ExpressQuiz.ViewModels;
using Microsoft.Ajax.Utilities;

namespace ExpressQuiz.Controllers
{
    public class UsersController : Controller
    {
        private IRepo<QuizResult> _quizResultRepo;

        public UsersController(
            IRepo<QuizResult> quizResultRepo
            )
        {


            _quizResultRepo = quizResultRepo;
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
           
                var results =_quizResultRepo.GetAll().Where(x => x.QuizId == quizId).DistinctBy(x=> x.UserId).OrderByDescending(x=>x.Score);
                
                return PartialView("_QuizUsersPartial", results.ToList());
              
            
        }
    }
}