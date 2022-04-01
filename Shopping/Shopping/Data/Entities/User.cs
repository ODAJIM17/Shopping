using Microsoft.AspNetCore.Identity;
using Shopping.Data.Entities;
using Shopping.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shopping.Data.Entities
{
    public class User : IdentityUser
    {

        [Display(Name = "First Name")]
        [MaxLength(50, ErrorMessage = "Field {0} allows max {1} characters")]
        [Required(ErrorMessage = "{0} is required.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(50, ErrorMessage = "Field {0} allows max {1} characters")]
        [Required(ErrorMessage = "{0} is required.")]
        public string LastName { get; set; }

        [Display(Name = "Address")]
        [MaxLength(200, ErrorMessage = "Field {0} allows max {1} characters")]
        [Required(ErrorMessage = "{0} is required.")]
        public string Address { get; set; }

        [Display(Name = "Photo")]
        public Guid ImageId { get; set; }

       
        [Display(Name = "Photo")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://localhost:7270/images/noimage.png"
            : $"https://shoppingZulu.blob.core.windows.net/users/{ImageId}";

        [Display(Name = "User Type")]
        public UserType UserType { get; set; }

        [Display(Name = "City")]
        public City City { get; set; }

        [Display(Name = "User")]
        public string FullName => $"{FirstName} {LastName}";

        //[Display(Name = "Usuario")]
        //public string FullNameWithDocument => $"{FirstName} {LastName} - {Document}";
    }
}