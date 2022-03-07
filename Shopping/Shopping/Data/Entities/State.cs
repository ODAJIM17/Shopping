using System.ComponentModel.DataAnnotations;

namespace Shopping.Data.Entities
{
    public class State
    {
        public int Id { get; set; }

        [Display(Name = "Estate")]
        [MaxLength(50, ErrorMessage = "Field {0} allows max {1} characters")]
        [Required(ErrorMessage = "Field {0} is required.")]
        public string Name { get; set; }

        public Country Country { get; set; }

        public ICollection<City> Cities { get; set; }

        [Display(Name = "Cities")]
        public int CitiesCount => Cities == null ? 0 : Cities.Count;
    }
}
