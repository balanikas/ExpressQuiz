using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExpressQuiz.Core.Models
{
    public class QuizResult : Entity
    {
        public virtual ICollection<UserAnswer> UserAnswers { get; set; }

        public int QuizId { get; set; }

        public virtual Quiz Quiz { get; set; }

        [Required]
        public int EllapsedTime { get; set; }

        [Required]
        public int Score { get; set; }

        [Required]
        public string UserId { get; set; }
       
    }
}