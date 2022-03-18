using System.ComponentModel.DataAnnotations;

namespace Shopping.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0} is required.")]
        [EmailAddress(ErrorMessage = "Invalid email.")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [Required(ErrorMessage = "{0} is required.")]
        [MinLength(6, ErrorMessage = "The field {0} must have ar least {1} characteres.")]
        public string Password { get; set; }

        [Display(Name = "Remember be")]
        public bool RememberMe { get; set; }
    }
}
