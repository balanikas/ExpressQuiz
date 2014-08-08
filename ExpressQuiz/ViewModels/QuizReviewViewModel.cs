using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.UI.WebControls.Expressions;
using ExpressQuiz.Models;
using ExpressQuiz.Repos;

namespace ExpressQuiz.ViewModels
{
    

    public class QuizReviewViewModel
    {

        public List<QuizReviewItem> Items { get; set; }

        public int Level { get; set; }
        public int Rating { get; set; }
        public QuizResult Result { get;  set; }
        public int QuizId { get; set; }

      
    }
}