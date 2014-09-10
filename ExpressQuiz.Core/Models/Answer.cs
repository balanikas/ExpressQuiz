using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExpressQuiz.Core.Models
{
    public class Answer : Entity
    {
 
        public int QuestionId { get; set; }

        public virtual Question Question { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        [StringLength(2000)]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        [StringLength(10000)]
        [DataType(DataType.MultilineText)]
        public string Explanation { get; set; }

        public bool IsCorrect { get; set; }


    }

   
}