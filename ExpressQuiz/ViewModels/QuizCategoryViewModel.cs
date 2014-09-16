using System.ComponentModel.DataAnnotations;

namespace ExpressQuiz.ViewModels
{
    public class QuizCategoryViewModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public int Id { get; set; }
        public bool IsSelected { get; set; }
        public int QuizCount { get; set; }
    }
}