using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

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