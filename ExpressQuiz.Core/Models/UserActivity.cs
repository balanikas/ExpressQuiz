using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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