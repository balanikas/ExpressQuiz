using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressQuiz.Tests.Services
{
    [TestClass]
    public abstract class ServiceTest
    {
        protected readonly MockRepository Mocks;

        public ServiceTest()
        {
            var uri = Path.Combine(Environment.CurrentDirectory, "testdata.xml");
            Mocks = new MockRepository(uri);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            Mocks.Reload();
        }
    }
}
