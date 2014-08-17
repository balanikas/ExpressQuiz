using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressQuiz.Tests.ViewModels
{
    [TestClass]
    public abstract class ViewModelTest
    {
        protected RepoProvider _repoProvider;

        [TestInitialize()]
        public void Initialize()
        {
            _repoProvider = new RepoProvider();

            var uri =
               @"C:\Users\grillo\Documents\GitHub\ExpressQuiz\ExpressQuiz\bin\App_Data\testdata.xml";
            _repoProvider.Load(uri);


        }
    }
}
