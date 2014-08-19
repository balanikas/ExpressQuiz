using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Models;

namespace ExpressQuiz.ViewModels
{
    public class QuizDetailsViewModel
    {
        public Quiz Quiz { get; set; }
        
        [DisplayName("Average score")]
        public int AvgScore { get; set; }

        [DisplayName("Difficulty")]
        public int AvgLevel { get; set; }
        [DisplayName("Rating")]
        public int AvgRating { get; set; }
        [DisplayName("Average time taken")]
        public int AvgTime { get; set; }

        [DisplayName("Attempts")]
        public int Sessions { get; set; }
        public int AvgScorePercent { get; set; }
        public int AvgTimePercent { get; set; }
    }
}