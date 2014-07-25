using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ExpressQuiz.Models
{
    public class Answer
    {
        public int ID { get; set; }
        public int QuestionID { get; set; }
        public virtual Question Question { get; set; }
      
        public string Text { get; set; }

        public string Explanation { get; set; }

        public bool IsCorrect { get; set; }


    }

   
}