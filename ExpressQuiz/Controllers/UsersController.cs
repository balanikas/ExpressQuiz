using System.Linq;
using System.Web.Mvc;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Repos;
using Microsoft.Ajax.Utilities;

namespace ExpressQuiz.Controllers
{
    public class UsersController : Controller
    {
        private readonly ModelConverter _modelConverter;
        private readonly IRepo<QuizResult> _quizResultRepo;

        public UsersController(
            IRepo<QuizResult> quizResultRepo, ModelConverter modelConverter)
        {
            _quizResultRepo = quizResultRepo;
            _modelConverter = modelConverter;
        }


        public ActionResult Index(int quizId)
        {
            var results = _quizResultRepo.GetAll()
                .Where(x => x.QuizId == quizId)
                .OrderByDescending(x => x.Score)
                .DistinctBy(x => x.UserId)
                .Take(5);

            var vm = results.ToList().Select(x => _modelConverter.ToQuizResultViewModel(x)).ToList();
            return PartialView("_QuizUsersPartial", vm);
        }
    }
}