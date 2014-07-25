using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace ExpressQuiz.Models
{
    public class Question
    {
        public int ID { get; set; }
        public QuestionType Type {get;set;}

     
        public string Text { get; set; }

        public int QuizID { get; set; }
        public virtual Quiz Quiz { get; set; }
        public virtual List<Answer> Answers { get; set; } 
    }


  
}