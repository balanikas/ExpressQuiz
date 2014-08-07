using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace ExpressQuiz.Models
{
    public class Quiz : Entity
    {
       


        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(1000)]
        public string Summary { get; set; }


        public bool IsTimeable { get; set; }

        //[DataType(DataType.DateTime)]
        [Column(TypeName = "DateTime2")]
        public DateTime Created { get; set; }

        public int QuizCategoryId { get; set; }
        public virtual QuizCategory Category { get; set; }
        public virtual ICollection<Question> Questions { get; set; }

        public Quiz()
        {
            Questions = new List<Question>();
        }
    }

  
}