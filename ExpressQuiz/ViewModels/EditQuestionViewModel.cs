using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpressQuiz.Core.Models;

namespace ExpressQuiz.ViewModels
{
    public class EditQuestionViewModel
    {
        public Question Question { get; set; }
        public string Order { get; set; }
      
    }
}