using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpressQuiz.ViewModels
{
    public class EditAnswerViewModel
    {
        public AnswerViewModel Answer { get; set; }
        public int QuestionId { get; set; }
        public int QuizId { get; set; }
    }
}