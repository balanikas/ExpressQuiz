using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpressQuiz.Core.Models;

namespace ExpressQuiz.ViewModels
{
    public class EditQuizViewModel
    {
        public QuizViewModel Quiz { get; set; }
        public string QuestionOrder { get; set; }

        [DisplayName("Category")]
        public int SelectedCategory { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }

        [DisplayName("How many seconds should it take?")]
        public uint EstimatedTime { get; set; }

         [DisplayName("Create new category")]
        public string NewCategory { get; set; }
  
    }
}