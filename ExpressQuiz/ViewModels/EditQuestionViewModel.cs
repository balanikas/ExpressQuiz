using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Models;

namespace ExpressQuiz.ViewModels
{
    public class EditQuestionViewModel
    {
        public Question Question { get; set; }
        public string Order { get; set; }
        public bool ModifiedByUser { get; set; }
    }
}