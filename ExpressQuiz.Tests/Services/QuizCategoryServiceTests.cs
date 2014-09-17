using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressQuiz.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressQuiz.Tests.Services
{
    [TestClass]
    public class QuizCategoryServiceTests : ServiceTest
    {
        [TestMethod]
        public void QuizCategoryService_GetAll()
        {
            var service = Mocks.QuizCategoryService;
            Assert.IsNotNull(service.GetAll());
        }

        [TestMethod]
        public void QuizCategoryService_Get()
        {
            var service = Mocks.QuizCategoryService;
            var cat = service.Get(1);
            Assert.IsNotNull(cat);
        }

        [TestMethod]
        public void QuizCategoryService_Insert()
        {
            var service = Mocks.QuizCategoryService;

            var cat = new QuizCategory();
            cat.Name = "name";

            var saved = service.Insert(cat);

            Assert.AreEqual(cat.Name, saved.Name);
            Assert.IsTrue(saved.Id > 0);

        }

        [TestMethod]
        public void QuizCategoryService_Update()
        {
            var service = Mocks.QuizCategoryService;

            var cat = service.Get(1);
            cat.Name = "name";

            service.Update(cat);

            var updated = service.Get(1);
            Assert.AreEqual(cat.Name, updated.Name);

        }

        [TestMethod]
        public void QuizCategoryService_Delete()
        {
            var service = Mocks.QuizCategoryService;
            service.Delete(1);
            Assert.IsNull(service.Get(1));
        }

        [TestMethod]
        public void QuizCategoryService_Exists()
        {
            var service = Mocks.QuizCategoryService;

            var cat = service.Get(1);
            cat.Name = "name";
            Assert.IsTrue(service.Exists("name"));
            Assert.IsTrue(service.Exists("Name"));

        }

        [TestMethod]
        public void QuizCategoryService_InsertByName()
        {
            var service = Mocks.QuizCategoryService;


            var cat = service.InsertByName("name");
            
            Assert.IsNotNull(service.Get(cat.Id));


        }

    }
}
