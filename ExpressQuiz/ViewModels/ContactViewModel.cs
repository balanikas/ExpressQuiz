using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExpressQuiz.ViewModels
{
    public class ContactViewModel
    {

        [DisplayName("Your email")]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }

        [StringLength(50)]
        [DisplayName("Your name")]
        [Required]
        public string Name { get; set; }

        [DisplayName("Message")]
        [Required]
        [StringLength(1000)]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }

        public bool WasSent { get; set; }

    }
}