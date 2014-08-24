using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ExpressQuiz.ViewModels
{
    public class QuizReviewItemViewModel
    {
       

        [DisplayName("Is correct")]
        public bool IsCorrectAnswer { get;  set; }

        [DisplayName("Question")]
        public string QuestionText { get;  set; }


        public int PointsEarned { get; set; }
        public int TimeTaken { get; set; }

        public int QuestionId { get;  set; }
    }
}