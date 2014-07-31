using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpressQuiz.Models;

namespace ExpressQuiz.ViewModels
{
    public class CreateQuizViewModel
    {
        public Quiz Quiz { get; set; }
        public int SelectedCategory { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; } 
    }
}