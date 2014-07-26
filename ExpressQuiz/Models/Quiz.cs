using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace ExpressQuiz.Models
{
    public class Quiz
    {
        public int ID { get; set; }
        public string Name { get; set; }

       // public int Rating { get; set; }

        [Required]
        public string Summary { get; set; }

        public virtual QuizCategory Category { get; set; }

        public virtual List<Question> Questions { get; set; } 
       
    }

  
}