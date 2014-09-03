using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpressQuiz.Core.Models;

namespace ExpressQuiz.ViewModels
{
    public class CreateQuizViewModel
    {
        public QuizViewModel Quiz { get; set; }
        [DisplayName("Category")]
        public int SelectedCategory { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }

        [DisplayName("Create new category")]
        public string NewCategory { get; set; }
    }
}