using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ExpressQuiz.Models
{
    public class QuizCategory : Entity
    {
       

        [Required]
        [StringLength(50)]
        public string Name  { get; set; }
    }

}