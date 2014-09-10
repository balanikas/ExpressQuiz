using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpressQuiz.Core.Models
{
    public class Quiz : Entity
    {

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(5000)]
        public string Summary { get; set; }

        public bool IsTimeable { get; set; }

        public bool AllowPoints { get; set; }

        [Column(TypeName = "DateTime2")]
        public DateTime Created { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        public bool Locked { get; set; }

        public int Views { get; set; }

        public int Votes { get; set; }

        public int Completed { get; set; }

        
        public virtual QuizCategory Category { get; set; }

        public virtual ICollection<Question> Questions { get; set; }

        public virtual ICollection<QuizRating> Ratings { get; set; }

        public virtual ICollection<QuizResult> Results { get; set; }

        public Quiz()
        {
            Questions = new List<Question>();
        }
    }

  
}