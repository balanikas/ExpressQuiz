using System.Collections.Generic;
using System.ComponentModel;

namespace ExpressQuiz.ViewModels
{
    public class QuizResultViewModel
    {
        public string UserId { get; set; }
        public int Score { get; set; }

        [DisplayName("Time")]
        public int EllapsedTime { get; set; }


        public int QuizId { get; set; }

        public List<UserAnswerViewModel> UserAnswers { get; set; }
    }
}