﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExpressQuiz.Core.Models
{
    public class Answer : Entity
    {
 
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }


        [Required]
        public int OrderId { get; set; }

        [Required]
        [StringLength(500)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Answer")]
        public string Text { get; set; }

        [StringLength(10000)]
        [DataType(DataType.MultilineText)]
        public string Explanation { get; set; }

        [DisplayName("Is it correct?")]
        public bool IsCorrect { get; set; }


    }

   
}