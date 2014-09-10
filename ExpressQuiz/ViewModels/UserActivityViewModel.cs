using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpressQuiz.ViewModels
{
    public class UserActivityViewModel
    {
     
        public string UserId { get; set; }
        public string Action { get; set; }
        public string Item { get; set; }
        public int ItemId { get; set; }
        public DateTime Date { get; set; }
    }
}