using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExpressQuiz.ViewModels
{
    public class QuizCategoryViewModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public int Id { get; set; }
        public bool IsSelected { get; set; }
        public int QuizCount { get; set; }
    }
}