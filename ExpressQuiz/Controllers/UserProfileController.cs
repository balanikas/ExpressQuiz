using System.Linq;
using System.Web.Mvc;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Services;
using ExpressQuiz.Extensions;
using ExpressQuiz.ViewModels;

namespace ExpressQuiz.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly IQuizService _quizService;
        public UserProfileController(IQuizService quizService)
        {
            _quizService = quizService;

        }

        public ActionResult Index(int? profileView)
        {
            var vm = new UserProfileViewModel();

            var quizzes = _quizService.GetAll().Where(x => x.CreatedBy == User.Identity.Name);
            vm.Quizzes = quizzes.ToQuizViewModels();

            vm.ProfileView = profileView.HasValue ? profileView.Value : 0;

            var socialSettings = new SocialSettingsViewModel();

            vm.SocialSettings = socialSettings;


            return View("Index",vm);
        }


        public ActionResult Public(string id)
        {
            var vm = new PublicUserProfileViewModel();
            vm.UserName = id;
            return View("Public", vm);
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
