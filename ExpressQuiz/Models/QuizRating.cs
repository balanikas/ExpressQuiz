using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace ExpressQuiz.Models
{
    public class QuizRating : Entity
    {
       
        [Required]
        public  int QuizId { get; set; }

        [Required]
        [Range(0,100)]
        public int Rating { get; set; }
    }
}