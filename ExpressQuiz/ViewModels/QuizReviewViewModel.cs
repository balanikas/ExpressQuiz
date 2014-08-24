using System;
using System.Collections.Generic;
using System.ComponentModel;

using ExpressQuiz.Core.Models;


namespace ExpressQuiz.ViewModels
{
    

    public class QuizReviewViewModel
    {

        public List<QuizReviewItemViewModel> Items { get; set; }

        [DisplayName("Difficulty")]
        public int Level { get; set; }
        [DisplayName("Rating")]
        public int Rating { get; set; }
        public QuizResult Result { get;  set; }
        public int QuizId { get; set; }
        public string ScoreText { get; set; }
        public int EllapsedTimePercent { get; set; }

        
    }
}