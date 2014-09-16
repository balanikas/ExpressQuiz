using System.Collections.Generic;

namespace ExpressQuiz.ViewModels
{
    public class UserQuizzesViewModel
    {
        public IEnumerable<QuizViewModel> Quizzes { get; set; }
        public string UserId { get; set; }
    }
}