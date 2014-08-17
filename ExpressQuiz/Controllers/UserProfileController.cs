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

namespace ExpressQuiz.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly IRepo<Quiz> _quizRepo;

        public UserProfileController(IRepo<Quiz> quizRepo)
        {
            _quizRepo = quizRepo;
        }

        public ActionResult Index(int? profileView)
        {
            var vm = new UserProfileViewModel();
            vm.Quizzes = _quizRepo.GetAll().Where(x => x.CreatedBy == User.Identity.Name);
            vm.ProfileView = profileView.HasValue ? profileView.Value : 0;

            var socialSettings = new SocialSettingsViewModel();

            vm.SocialSettings = socialSettings;


            return View(vm);
        }

        [HttpPost]
        public ActionResult EditSocialSettings(SocialSettingsViewModel model)
        {
            if (ModelState.IsValid)
            {
                
            }

            return PartialView("_SocialPartial",model);

        }
    }
}
