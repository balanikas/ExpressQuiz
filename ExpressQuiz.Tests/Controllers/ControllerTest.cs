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
        protected RepoProvider _repoProvider;
        protected ServiceProvider _serviceProvider;
        protected ControllerProvider _controllerProvider;
        [TestInitialize()]
        public void Initialize()
        {
            _repoProvider = new RepoProvider();

            var uri =
               @"C:\Users\grillo\Documents\GitHub\ExpressQuiz\ExpressQuiz\bin\App_Data\testdata.xml";
            _repoProvider.Load(uri);

            _serviceProvider = new ServiceProvider(_repoProvider);
            _controllerProvider = new ControllerProvider(_repoProvider, _serviceProvider);
        }
    }
}
