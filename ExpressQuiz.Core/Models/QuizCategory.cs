using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpressQuiz.Core.Models
{
    public class QuizCategory : Entity
    {

        [Index("NameIndex", IsUnique = true)]
        [Required]
        [StringLength(50)]
        public string Name  { get; set; }
    }

}