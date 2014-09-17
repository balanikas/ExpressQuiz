using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExpressQuiz;
using ExpressQuiz.Controllers;

namespace ExpressQuiz.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest : ControllerTest
    {
        [TestMethod]
        public void Index()
        {
            var controller = ControllerProvider.CreateHomeController();
            var result = controller.Index() as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void About()
        {
            var controller = ControllerProvider.CreateHomeController();
            var result = controller.About() as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Contact()
        {
            var controller = ControllerProvider.CreateHomeController();
            var result = controller.Contact() as ViewResult;
            Assert.IsNotNull(result);
        }
    }
}
