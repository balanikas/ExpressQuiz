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
    public class QuizViewModel
    {

        public int QuizId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(1000)]
        public string Summary { get; set; }

        [DisplayName("Use timer")]
        public bool IsTimeable { get; set; }

        [DisplayName("Allow question points")]
        public bool AllowPoints { get; set; }

        [Column(TypeName = "DateTime2")]
        public DateTime Created { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        [DisplayName("Locked")]
        public bool Locked { get; set; }

        public IList<QuestionViewModel> Questions { get; set; }

        public QuizCategoryViewModel Category { get; set; }
    }
}