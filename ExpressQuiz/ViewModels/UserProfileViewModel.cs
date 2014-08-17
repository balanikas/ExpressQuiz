using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExpressQuiz.Models;

namespace ExpressQuiz.ViewModels
{
    public class UserProfileViewModel
    {
        public IEnumerable<Quiz> Quizzes { get; set; }
        public int ProfileView { get; set; }
        public SocialSettingsViewModel SocialSettings { get; set; }
        
    }
}