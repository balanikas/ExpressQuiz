using System.ComponentModel.DataAnnotations;

namespace ExpressQuiz.Core.Models
{
    public class QuizRating : Entity
    {
        [Required]
        public int QuizId { get; set; }

        public virtual Quiz Quiz { get; set; }

        [Required]
        [Range(0, 100)]
        public int Rating { get; set; }

        [Required]
        [Range(0, 100)]
        public int Level { get; set; }
    }
}