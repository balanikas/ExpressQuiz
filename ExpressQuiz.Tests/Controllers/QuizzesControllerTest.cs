using System.Net;
using System.Web.Mvc;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Extensions;
using ExpressQuiz.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressQuiz.Tests.Controllers
{
    [TestClass]
    public class QuizzesControllerTest : ControllerTest
    {
        [TestMethod]
        public void Index()
        {

            var c = _controllerProvider.CreateQuizzesController();


            QuizzesViewModel model;
            ViewResult result;

            result = c.Index(null, "") as ViewResult;
            model = result.Model as QuizzesViewModel;

            Assert.IsNotNull(model);
            Assert.AreEqual(2,model.Quizzes.Count);
            Assert.AreEqual(3,model.QuizCategories.Count);

            result = c.Index(null, "__") as ViewResult;
            model = result.Model as QuizzesViewModel;
            Assert.AreEqual(0, model.Quizzes.Count);

            result = c.Index(null, "qui") as ViewResult;
            model = result.Model as QuizzesViewModel;
            Assert.AreEqual(2, model.Quizzes.Count);
        }

      
        [TestMethod]
        public void Details()
        {
            var c = _controllerProvider.CreateQuizzesController();

            var result = c.Details(1) as ViewResult;
            var model = result.Model as QuizDetailsViewModel;
            Assert.IsNotNull(model);

        }

        [TestMethod]
        public void Details_Resource_Not_Found()
        {
            var c = _controllerProvider.CreateQuizzesController();

            HttpStatusCodeResult result;

            result = c.Details(-1) as HttpStatusCodeResult;

            Assert.AreEqual(result.StatusCode, new HttpStatusCodeResult(HttpStatusCode.NotFound).StatusCode);

        }

        [TestMethod]
        public void Details_Bad_Request()
        {
            var c = _controllerProvider.CreateQuizzesController();

            HttpStatusCodeResult result;

            result = c.Details(null) as HttpStatusCodeResult;

            Assert.AreEqual(result.StatusCode, new HttpStatusCodeResult(HttpStatusCode.BadRequest).StatusCode);

        }


        [TestMethod]
        public void Edit_Get()
        {
            var c = _controllerProvider.CreateQuizzesController();

            var result = c.Edit(1) as ViewResult;
            var model = result.Model as EditQuizViewModel;

            Assert.IsNotNull(model);
        }

        [TestMethod]
        public void Edit_Bad_Request()
        {
            var c = _controllerProvider.CreateQuizzesController();

            HttpStatusCodeResult result;

            result = c.Edit((int?) null) as HttpStatusCodeResult;

            Assert.AreEqual(result.StatusCode, new HttpStatusCodeResult(HttpStatusCode.BadRequest).StatusCode);
        }

        [TestMethod]
        public void Edit_Not_Found()
        {
            var c = _controllerProvider.CreateQuizzesController();

            HttpStatusCodeResult result;

            result = c.Edit(-1) as HttpStatusCodeResult;

            Assert.AreEqual(result.StatusCode, new HttpStatusCodeResult(HttpStatusCode.NotFound).StatusCode);
        }

        [TestMethod]
        public void Edit_Post()
        {
            var c = _controllerProvider.CreateQuizzesController();
            EditQuizViewModel model = _mockRepository.QuizService.Get(1).ToViewModel(_mockRepository.QuizCategoryService);

            var result = c.Edit(model) as PartialViewResult;
            model = result.Model as EditQuizViewModel;

            Assert.IsNotNull(model);
        }

        [TestMethod]
        public void EditQuestion_Get()
        {
            var c = _controllerProvider.CreateQuizzesController();
            
            var result = c.EditQuestion(1) as PartialViewResult;
            var model = result.Model as EditQuestionViewModel;

            Assert.IsNotNull(model);
        }

        [TestMethod]
        public void EditQuestion_Bad_Request()
        {
            var c = _controllerProvider.CreateQuizzesController();

            HttpStatusCodeResult result;

            result = c.EditQuestion((int?)null) as HttpStatusCodeResult;

            Assert.AreEqual(result.StatusCode, new HttpStatusCodeResult(HttpStatusCode.BadRequest).StatusCode);
        }

        [TestMethod]
        public void EditQuestion_Not_Found()
        {
            var c = _controllerProvider.CreateQuizzesController();

            HttpStatusCodeResult result;

            result = c.EditQuestion(-1) as HttpStatusCodeResult;

            Assert.AreEqual(result.StatusCode, new HttpStatusCodeResult(HttpStatusCode.NotFound).StatusCode);
        }

        [TestMethod]
        public void EditQuestion_Post()
        {
            var c = _controllerProvider.CreateQuizzesController();
            EditQuestionViewModel model = _mockRepository.QuestionRepo.Get(1).ToViewModel();

            PartialViewResult result;

            result = c.EditQuestion(model) as PartialViewResult;
            model = result.Model as EditQuestionViewModel;

            Assert.IsNotNull(model);
        }




        [TestMethod]
        public void EditAnswer_Get()
        {
            var c = _controllerProvider.CreateQuizzesController();

            var result = c.EditAnswer(1) as PartialViewResult;
            var model = result.Model as Answer;

            Assert.IsNotNull(model);
        }

        [TestMethod]
        public void EditAnswer_Bad_Request()
        {
            var c = _controllerProvider.CreateQuizzesController();

            HttpStatusCodeResult result;

            result = c.EditAnswer((int?)null) as HttpStatusCodeResult;

            Assert.AreEqual(result.StatusCode, new HttpStatusCodeResult(HttpStatusCode.BadRequest).StatusCode);
        }

        [TestMethod]
        public void EditAnswer_Not_Found()
        {
            var c = _controllerProvider.CreateQuizzesController();

            HttpStatusCodeResult result;

            result = c.EditAnswer(-1) as HttpStatusCodeResult;

            Assert.AreEqual(result.StatusCode, new HttpStatusCodeResult(HttpStatusCode.NotFound).StatusCode);
        }

        [TestMethod]
        public void EditAnswer_Post()
        {
            var c = _controllerProvider.CreateQuizzesController();
            Answer model = _mockRepository.AnswerRepo.Get(1);

            PartialViewResult result;

            result = c.EditAnswer(model) as PartialViewResult;
            var returnedModel = result.Model as EditQuestionViewModel;

            Assert.IsNotNull(returnedModel);
        }


        //[TestMethod]
        //public void Create_Get()
        //{
        //    var c = _controllerProvider.CreateQuizzesController();
        //    var result = c.Create() as ViewResult;
        //    var model = result.Model as CreateQuizViewModel;

        //    Assert.IsNotNull(model);

        //}

        //[TestMethod]
        //public void Create_Post()
        //{
        //    var c = _controllerProvider.CreateQuizzesController();

        //    CreateQuizViewModel model = _mockRepository.QuizRepo.Get(1).ToViewModel(_mockRepository.QuizCategoryRepo, "username");


        //    var result = c.Create(model) as ViewResult;
        //    model = result.Model as CreateQuizViewModel;

        //    Assert.IsNotNull(model);

        //}


        [TestMethod]
        public void CreateQuestion_Get()
        {
            var c = _controllerProvider.CreateQuizzesController();
            var result = c.CreateQuestion(1,1) as PartialViewResult;
            var model = result.Model as EditQuizViewModel;

            Assert.IsNotNull(model);

        }


        [TestMethod]
        public void CreateQuestion_Bad_Request()
        {
            var c = _controllerProvider.CreateQuizzesController();
            var result = c.CreateQuestion(null, 1) as HttpStatusCodeResult;
            Assert.AreEqual(result.StatusCode, new HttpStatusCodeResult(HttpStatusCode.BadRequest).StatusCode);

        }

        [TestMethod]
        public void CreateQuestion_Not_Found()
        {
            var c = _controllerProvider.CreateQuizzesController();
            var result = c.CreateQuestion(-1, 1) as HttpStatusCodeResult;
            Assert.AreEqual(result.StatusCode, new HttpStatusCodeResult(HttpStatusCode.NotFound).StatusCode);

        }

        [TestMethod]
        public void CreateAnswer_Get()
        {
            var c = _controllerProvider.CreateQuizzesController();
            var result = c.CreateAnswer(1, 1) as PartialViewResult;
            var model = result.Model as EditQuestionViewModel;

            Assert.IsNotNull(model);

        }


        [TestMethod]
        public void CreateAnswer_Bad_Request()
        {
            var c = _controllerProvider.CreateQuizzesController();
            var result = c.CreateAnswer(null, 1) as HttpStatusCodeResult;
            Assert.AreEqual(result.StatusCode, new HttpStatusCodeResult(HttpStatusCode.BadRequest).StatusCode);

        }

        [TestMethod]
        public void CreateAnswer_Not_Found()
        {
            var c = _controllerProvider.CreateQuizzesController();
            var result = c.CreateAnswer(-1, 1) as HttpStatusCodeResult;
            Assert.AreEqual(result.StatusCode, new HttpStatusCodeResult(HttpStatusCode.NotFound).StatusCode);

        }

        [TestMethod]
        public void Delete()
        {
            var c = _controllerProvider.CreateQuizzesController();
            var result = c.Delete(1) as ViewResult;
            var model = result.Model as Quiz;

            Assert.IsNotNull(model);

        }

        [TestMethod]
        public void Delete_Bad_Request()
        {
            var c = _controllerProvider.CreateQuizzesController();
            var result = c.Delete(null) as HttpStatusCodeResult;
            Assert.AreEqual(result.StatusCode, new HttpStatusCodeResult(HttpStatusCode.BadRequest).StatusCode);

        }

        [TestMethod]
        public void Delete_Not_Found()
        {
            var c = _controllerProvider.CreateQuizzesController();
            var result = c.Delete(-1) as HttpStatusCodeResult;
            Assert.AreEqual(result.StatusCode, new HttpStatusCodeResult(HttpStatusCode.NotFound).StatusCode);

        }

        [TestMethod]
        public void DeleteQuestion()
        {
            var c = _controllerProvider.CreateQuizzesController();
            var result = c.DeleteQuestion(1) as PartialViewResult;
            var model = result.Model as EditQuizViewModel;

            Assert.IsNotNull(model);

        }

        [TestMethod]
        public void DeleteQuestion_Bad_Request()
        {
            var c = _controllerProvider.CreateQuizzesController();
            var result = c.DeleteQuestion(null) as HttpStatusCodeResult;
            Assert.AreEqual(result.StatusCode, new HttpStatusCodeResult(HttpStatusCode.BadRequest).StatusCode);

        }

        [TestMethod]
        public void DeleteQuestion_Not_Found()
        {
            var c = _controllerProvider.CreateQuizzesController();
            var result = c.DeleteQuestion(-1) as HttpStatusCodeResult;
            Assert.AreEqual(result.StatusCode, new HttpStatusCodeResult(HttpStatusCode.NotFound).StatusCode);

        }

        [TestMethod]
        public void DeleteAnswer()
        {
            var c = _controllerProvider.CreateQuizzesController();
            var result = c.DeleteAnswer(1) as PartialViewResult;
            var model = result.Model as EditQuestionViewModel;

            Assert.IsNotNull(model);

        }

        [TestMethod]
        public void DeleteAnswer_Bad_Request()
        {
            var c = _controllerProvider.CreateQuizzesController();
            var result = c.DeleteAnswer(null) as HttpStatusCodeResult;
            Assert.AreEqual(result.StatusCode, new HttpStatusCodeResult(HttpStatusCode.BadRequest).StatusCode);

        }

        [TestMethod]
        public void DeleteAnswer_Not_Found()
        {
            var c = _controllerProvider.CreateQuizzesController();
            var result = c.DeleteAnswer(-1) as HttpStatusCodeResult;
            Assert.AreEqual(result.StatusCode, new HttpStatusCodeResult(HttpStatusCode.NotFound).StatusCode);

        }
    }
}
