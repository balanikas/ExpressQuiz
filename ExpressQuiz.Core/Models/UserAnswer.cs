using System.ComponentModel.DataAnnotations;

namespace ExpressQuiz.Core.Models
{
    public class UserAnswer : Entity
    {
       
        [Required]
        public int AnswerId { get; set; }

        [Required]
        public int QuestionId { get; set; }

        public int QuizResultId { get; set; }

        public virtual QuizResult QuizResult { get; set; }
    }
}