using System.Collections.Generic;
using System.ComponentModel;


namespace ExpressQuiz.ViewModels
{
    

    public class QuizReviewViewModel
    {

        public List<QuizReviewItemViewModel> Items { get; set; }

        [DisplayName("Difficulty")]
        public int Level { get; set; }
        [DisplayName("Rating")]
        public int Rating { get; set; }


        public int Score { get; set; }
        public int EllapsedTime { get; set; }
        public int ResultId { get; set; }
        public List<UserAnswerViewModel> UserAnswers { get; set; } 

        public int QuizId { get; set; }
        public string ScoreText { get; set; }
        public int EllapsedTimePercent { get; set; }
        public int RelativeScore { get; set; }
        public int RelativeTime { get; set; }
        
    }
}