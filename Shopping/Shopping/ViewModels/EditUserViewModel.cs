using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Shopping.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }


        [Display(Name = "First Name")]
        [MaxLength(50, ErrorMessage = " {0} Max allowed {1} characters.")]
        [Required(ErrorMessage = "{0} is required.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(50, ErrorMessage = " {0} Max allowed {1} characters.")]
        [Required(ErrorMessage = "{0} is required.")]
        public string LastName { get; set; }

        [Display(Name = "Address")]
        [MaxLength(200, ErrorMessage = " {0} Max allowed {1} characters.")]
        [Required(ErrorMessage = "{0} is required.")]
        public string Address { get; set; }

        [Display(Name = "Teléfono")]
        [MaxLength(20, ErrorMessage = " {0} Max allowed {1} characters.")]
        [Required(ErrorMessage = "{0} is required.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Photo")]
        public Guid ImageId { get; set; }

        [Display(Name = "Photo")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://localhost:7270/images/noimage.png"
            : $"https://shoppingzulu.blob.core.windows.net/users/{ImageId}";

        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }

        [Display(Name = "Country")]
        [Range(1, int.MaxValue, ErrorMessage = "Select a country.")]
        [Required(ErrorMessage = "{0} is required.")]
        public int CountryId { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }

        [Display(Name = "Estate")]
        [Range(1, int.MaxValue, ErrorMessage = "Select state.")]
        [Required(ErrorMessage = "{0} is required.")]
        public int StateId { get; set; }

        public IEnumerable<SelectListItem> States { get; set; }

        [Display(Name = "City")]
        [Range(1, int.MaxValue, ErrorMessage = "Select a city.")]
        public int CityId { get; set; }

        public IEnumerable<SelectListItem> Cities { get; set; }
    }
}
