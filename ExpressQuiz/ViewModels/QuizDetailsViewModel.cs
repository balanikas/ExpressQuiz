using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using ExpressQuiz.Core.Models;

namespace ExpressQuiz.ViewModels
{
    public class QuizDetailsViewModel
    {
        public QuizViewModel Quiz { get; set; }
        
        [DisplayName("Average score")]
        public int AvgScore { get; set; }

        [DisplayName("Difficulty")]
        public int AvgLevel { get; set; }
        [DisplayName("Rating")]
        public int AvgRating { get; set; }
        [DisplayName("Average time taken")]
        public int AvgTime { get; set; }

        public int AvgTimePercent { get; set; }
        public int AvgScorePercent { get; set; }
        public int TotalPoints { get; set; }
        public int TotalTime { get; set; }


    }
}