using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpressQuiz.ViewModels
{
    public class QuizCategoryViewModel
    {
       
        public string Name { get; set; }
        public int Id { get; set; }
        public bool IsSelected { get; set; }
        public int QuizCount { get; set; }
    }
}