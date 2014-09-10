using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpressQuiz.Core.Models
{
    public class Question : Entity
    {

        [Required]
        [StringLength(5000)]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Range(1, 600)]
        [Column(TypeName = "int")]
        public int EstimatedTime { get; set; }

        [Range(0,10)]
        public int Points { get; set; }

        public int QuizId { get; set; }

        public virtual Quiz Quiz { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }

        public int Votes { get; set; }

        public Question()
        {
            Answers = new List<Answer>();
        }
    }


  
}