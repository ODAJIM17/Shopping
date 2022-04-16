using System.ComponentModel.DataAnnotations;

namespace Shopping.ViewModels
{
    public class ResendTokenViewModel
    {
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email.")]
        [MaxLength(100, ErrorMessage = "{0} Max allowed {1} characters.")]
        [Required(ErrorMessage = "{0} is required.")]
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
