using System.ComponentModel.DataAnnotations;

namespace Shopping.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email.")]
        [Required(ErrorMessage = "{0} is required.")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Field {0} allows between {2} and {1} characters.")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password and confirm password do not match.")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Field {0} allows between {2} and {1} characters.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public string Token { get; set; }
    }
}
