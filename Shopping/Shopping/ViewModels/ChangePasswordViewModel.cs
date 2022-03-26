using System.ComponentModel.DataAnnotations;

namespace Shopping.ViewModels
{
    public class ChangePasswordViewModel
    {
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        [MaxLength(20, ErrorMessage = " {0} Max allowed {1} characters.")]
        [Required(ErrorMessage = "{0} is required.")]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [MaxLength(20, ErrorMessage = " {0} Max allowed {1} characters.")]
        [Required(ErrorMessage = "{0} is required.")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Password and confirm password do not match.")]
        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Field {0} allows between {2} y {1} characters.")]
        public string Confirm { get; set; }
    }
}
