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
        protected MockRepository _mockRepository;

        [TestInitialize()]
        public void Initialize()
        {
            

            var uri =
               @"C:\Users\grillo\Documents\GitHub\ExpressQuiz\ExpressQuiz\bin\App_Data\seeddata.xml";
            _mockRepository = new MockRepository(uri);
           


        }
    }
}
