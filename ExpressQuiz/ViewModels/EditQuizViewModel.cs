using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpressQuiz.Models;

namespace ExpressQuiz.ViewModels
{
    public class EditQuizViewModel
    {
        public Quiz Quiz { get; set; }
        public string Order { get; set; }
        public int SelectedCategory { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; } 
    }
}