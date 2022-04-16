using System.ComponentModel.DataAnnotations;

namespace Shopping.ViewModels
{
    public class AddProductImageViewModel
    {

        public int ProductId { get; set; }

        [Display(Name = "Photo")]
        [Required(ErrorMessage = "{0} is required.")]
        public IFormFile ImageFile { get; set; }
    }
}
