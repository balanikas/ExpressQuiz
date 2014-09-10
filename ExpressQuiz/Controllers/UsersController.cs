using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Repos;
using ExpressQuiz.Extensions;
using ExpressQuiz.ViewModels;
using Microsoft.Ajax.Utilities;

namespace ExpressQuiz.Controllers
{
    public class UsersController : Controller
    {
        private readonly IRepo<QuizResult> _quizResultRepo;

        public UsersController(
            IRepo<QuizResult> quizResultRepo
            )
        {


            _quizResultRepo = quizResultRepo;
        }


        public ActionResult Index(int quizId)
        {

            var results = _quizResultRepo.GetAll()
                .Where(x => x.QuizId == quizId)
                .OrderByDescending(x => x.Score)
                .DistinctBy(x => x.UserId)
                .Take(5);

            var vm = results.ToList().Select(x => x.ToQuizResultViewModel()).ToList();
            return PartialView("_QuizUsersPartial", vm);


        }
    }
}