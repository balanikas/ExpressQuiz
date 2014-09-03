using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ExpressQuiz.Core.Models;

namespace ExpressQuiz.ViewModels
{
    public class QuestionViewModel
    {
        public int QuestionId { get; set; }

        [Required]
        [StringLength(1000)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Question")]
        public string Text { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Range(1, 600)]
        [Column(TypeName = "int")]
        [DisplayName("How many seconds should it take?")]
        public int EstimatedTime { get; set; }

        [Range(0, 1000)]
        [DisplayName("Assign points")]
        public int Points { get; set; }

        public IList<AnswerViewModel> Answers { get; set; }

        public QuestionViewModel()
        {
            Answers = new List<AnswerViewModel>();
        }

    }
}