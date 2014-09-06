using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExpressQuiz.ViewModels
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DisplayName("Username")]
        public string NickName { get; set; }
    }
}