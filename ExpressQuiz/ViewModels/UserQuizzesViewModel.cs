using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpressQuiz.ViewModels
{
    public class UserQuizzesViewModel
    {
        public IEnumerable<QuizViewModel> Quizzes { get; set; }
        public string UserId { get; set; }
    }
}