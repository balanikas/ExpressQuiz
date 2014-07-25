using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace ExpressQuiz.Models
{
    public class QuizRating
    {
        public int Id { get; set; }

        public  Quiz Quiz { get; set; }
        public int Rating { get; set; }
    }
}