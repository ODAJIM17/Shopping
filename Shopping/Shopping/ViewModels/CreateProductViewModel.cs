using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Shopping.ViewModels
{
    public class CreateProductViewModel : EditProductViewModel
    {
        [Display(Name = "Category")]
        [Range(1, int.MaxValue, ErrorMessage = "Select a categoty.")]
        [Required(ErrorMessage = "{0} is required.")]
        public int CategoryId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }

        [Display(Name = "Photo")]
        public IFormFile? ImageFile { get; set; }
    }
}
