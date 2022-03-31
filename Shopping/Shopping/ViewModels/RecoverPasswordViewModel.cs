using System.ComponentModel.DataAnnotations;

namespace Shopping.ViewModels
{
    public class RecoverPasswordViewModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0} is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email.")]
        public string Email { get; set; }
    }
}
