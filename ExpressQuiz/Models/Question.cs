using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace ExpressQuiz.Models
{
    public class Question : Entity
    {
       


        [Required]
        [StringLength(1000)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Question text")]
        public string Text { get; set; }

        [Required]
        public int OrderId { get; set; }

        //[Range(typeof(int),"1", "600")]
        [Column(TypeName = "int")]
        public int EstimatedTime { get; set; }

        [Range(0,1000)]
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