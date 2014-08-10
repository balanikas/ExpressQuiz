using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExpressQuiz.Models;

namespace ExpressQuiz.ViewModels
{
    public class QuizDetailsViewModel
    {
        public Quiz Quiz { get; set; }
        public int AvgScore { get; set; }
        public int AvgLevel { get; set; }
        public int AvgRating { get; set; }
        public int AvgTime { get; set; }
        public int Sessions { get; set; }


    }
}