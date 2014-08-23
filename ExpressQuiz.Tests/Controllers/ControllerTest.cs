using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressQuiz.Tests.Controllers
{
    [TestClass]
    public abstract class ControllerTest
    {

        protected MockRepository _mockRepository;
        protected ControllerProvider _controllerProvider;
        [TestInitialize()]
        public void Initialize()
        {
           

            var uri =
               @"C:\Users\grillo\Documents\GitHub\ExpressQuiz\ExpressQuiz\bin\App_Data\seeddata.xml";
            _mockRepository = new MockRepository(uri);

          
            _controllerProvider = new ControllerProvider(_mockRepository);
        }
    }
}
