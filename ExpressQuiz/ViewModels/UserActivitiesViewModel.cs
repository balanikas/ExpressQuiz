using System.Collections.Generic;

namespace ExpressQuiz.ViewModels
{
    public class UserActivitiesViewModel
    {
        public string UserId { get; set; }
        public IEnumerable<UserActivityViewModel> Activities { get; set; }
    }
}