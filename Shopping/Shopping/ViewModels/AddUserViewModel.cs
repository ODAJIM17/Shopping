using Shopping.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shopping.ViewModels
{
    public class AddUserViewModel : EditUserViewModel
    {
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Please enter a validemail.")]
        [MaxLength(100, ErrorMessage = " {0} Max allowed {1} characters.")]
        [Required(ErrorMessage = "{0} is required.")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Field {0} allows between {2} and {1} characters.")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password and confirm password do not match.")]
        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Field {0} allows between {2} y {1} characters.")]
        public string PasswordConfirm { get; set; }

        [Display(Name = "User Type")]
        public UserType UserType { get; set; }
    }
}
