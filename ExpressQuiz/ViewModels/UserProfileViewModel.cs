﻿namespace ExpressQuiz.ViewModels
{
    public class UserProfileViewModel
    {
        public UserQuizzesViewModel Quizzes { get; set; }
        public UserActivitiesViewModel UserActivities { get; set; }
        public int ProfileView { get; set; }
        public SocialSettingsViewModel SocialSettings { get; set; }
        public string UserId { get; set; }
    }
}