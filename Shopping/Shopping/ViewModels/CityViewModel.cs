using Shopping.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Shopping.ViewModels
{
    public class CityViewModel
    {
        public int Id { get; set; }

        [Display(Name = "City")]
        [MaxLength(50, ErrorMessage = "Field {0} allows max {1} characters")]
        [Required(ErrorMessage = "Field {0} is required.")]
        public string Name { get; set; }
        public int StateId { get; set; }
    }
}
