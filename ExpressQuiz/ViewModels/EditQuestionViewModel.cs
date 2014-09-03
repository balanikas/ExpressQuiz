using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpressQuiz.Core.Models;

namespace ExpressQuiz.ViewModels
{
    public class EditQuestionViewModel
    {
        public QuestionViewModel Question { get; set; }
        public string Order { get; set; }
        public int QuizId { get; set; }
        public bool AllowPoints { get; set; }
        public bool IsTimeable { get; set; }
    }
}