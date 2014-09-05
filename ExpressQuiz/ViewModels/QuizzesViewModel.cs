using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpressQuiz.Core.Models;

namespace ExpressQuiz.ViewModels
{
    public class QuizzesViewModel
    {
        public IEnumerable<QuizCategoryViewModel> QuizCategories { get; set; }

        public IEnumerable<TopListItem> TopQuizzes { get; set; } 

        public IEnumerable<QuizViewModel> Quizzes { get; set; }

        public int SelectedFilter { get; set; }
        public IEnumerable<SelectListItem> Filters { get; set; }
        public QuizFilter Filter { get; set; }

        public int SelectedCategoryId { get; set; }
        public int PageCount { get; set; }
        public string SearchPlaceHolder { get; set; }
    }
}