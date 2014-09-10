using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressQuiz.Core.Models
{
    public class UserActivity : Entity
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public ActivityAction Action { get; set; }

        [Required]
        public ActivityItem Item { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Column(TypeName = "DateTime2")]
        [Required]
        public DateTime Date { get; set; }
    }
}
