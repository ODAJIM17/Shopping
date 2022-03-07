using System.ComponentModel.DataAnnotations;

namespace Shopping.ViewModels
{
    public class StateViewModel
    {
        public int Id { get; set; }

        [Display(Name = "State")]
        [MaxLength(50, ErrorMessage = "Field {0} allows max {1} characters")]
        [Required(ErrorMessage = "Filed {0} is required.")]
        public string Name { get; set; }

        public int CountryId { get; set; }
    }
}
