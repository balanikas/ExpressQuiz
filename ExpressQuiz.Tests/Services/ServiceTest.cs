using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressQuiz.Tests.Services
{
    [TestClass]
    public abstract class ServiceTest
    {
        protected readonly MockRepository _mockRepo;

        public ServiceTest()
        {
            var uri =
              @"C:\Users\grillo\Documents\GitHub\ExpressQuiz\ExpressQuiz\bin\App_Data\seeddata.xml";
            _mockRepo = new MockRepository(uri);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepo.Reload();
        }
    }
}
