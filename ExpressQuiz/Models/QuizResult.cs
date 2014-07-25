using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpressQuiz.Models
{
    public class QuizResult
    {

        public int ID { get; set; }
        public virtual ICollection<UserAnswer> Answers { get; set; }

        public int QuizId { get; set; }
        public virtual Quiz Quiz { get; set; }

        //public QuizResult()
        //{
        //    Answers = new List<UserAnswer>();
        //}
    }
}