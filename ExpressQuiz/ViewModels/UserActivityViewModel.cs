using System;

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