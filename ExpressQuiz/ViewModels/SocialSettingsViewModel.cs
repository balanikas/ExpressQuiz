using System.ComponentModel;

namespace ExpressQuiz.ViewModels
{
    public class SocialSettingsViewModel
    {
        [DisplayName("Recieve emails")]
        public bool SendEmail { get; set; }

        [DisplayName("When someone attempted one of your quizzes")]
        public bool OnQuizAttempt { get; set; }
    }
}