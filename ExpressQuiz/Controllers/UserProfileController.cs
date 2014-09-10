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
        private readonly IUserActivityService _userActivityService;

        public UserProfileController(IQuizService quizService, IUserActivityService userActivityService)
        {
            _quizService = quizService;
            _userActivityService = userActivityService;
        }

        public ActionResult Index(string id, int? profileView)
        {
            var vm = new UserProfileViewModel();

            vm.UserId = id;
            vm.ProfileView = profileView.HasValue ? profileView.Value : 0;

            var quizzes = _quizService.GetAll().Where(x => x.CreatedBy == id);
            vm.Quizzes = quizzes.ToUserQuizzesViewModel();
            vm.Quizzes.UserId = id;

            var socialSettings = new SocialSettingsViewModel();
            vm.SocialSettings = socialSettings;

            var userActivities = _userActivityService.GetAll(id);
            vm.UserActivities = userActivities.ToUserActivitiesViewModel();
            vm.UserActivities.UserId = id;

           

            return View("Index",vm);
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
