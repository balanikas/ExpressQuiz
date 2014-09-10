using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpressQuiz.ViewModels
{
    public class UserActivitiesViewModel
    {
        public string UserId { get; set; }
        public IEnumerable<UserActivityViewModel> Activities { get; set; }
    }
}