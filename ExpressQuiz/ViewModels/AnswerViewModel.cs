using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExpressQuiz.ViewModels
{
    public class AnswerViewModel
    {

        public int AnswerId { get; set; }

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