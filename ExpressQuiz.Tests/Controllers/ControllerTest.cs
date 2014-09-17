using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressQuiz.Tests.Controllers
{
    [TestClass]
    public abstract class ControllerTest
    {

        protected MockRepository Mocks;
        protected ControllerProvider ControllerProvider;
        [TestInitialize()]
        public void Initialize()
        {


            var uri = Path.Combine(Environment.CurrentDirectory, "testdata.xml");
            Mocks = new MockRepository(uri);

          
            ControllerProvider = new ControllerProvider(Mocks);
        }
    }
}
