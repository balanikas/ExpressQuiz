using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpressQuiz.Core.Models
{
    public class Question : Entity
    {

        //public int X { get; set; }

        [Required]
        [StringLength(1000)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Question")]
        public string Text { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Range(1, 600)]
        [Column(TypeName = "int")]
        [DisplayName("How many seconds should it take?")]
        public int EstimatedTime { get; set; }

        [Range(0,1000)]
        [DisplayName("Assign points")]
        public int Points { get; set; }

        public int QuizId { get; set; }
        public virtual Quiz Quiz { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
       // public int TypeId { get; set; }
        public virtual QuestionType Type { get; set; }

        public Question()
        {
            Answers = new List<Answer>();
        }
    }


  
}