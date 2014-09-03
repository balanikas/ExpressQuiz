using System.Collections.Generic;
using System.ComponentModel;

namespace ExpressQuiz.Core.Models
{
    public class QuizResult : Entity
    {

   
        public virtual ICollection<UserAnswer> Answers { get; set; }

        public int QuizId { get; set; }
        public virtual Quiz Quiz { get; set; }

        
        public int EllapsedTime { get; set; }
        public int Score { get; set; }
        public string UserId { get; set; }
       
    }
}