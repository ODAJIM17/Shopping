using System.ComponentModel.DataAnnotations;

namespace Shopping.Data.Entities
{
    public class Country
    {
        public int Id { get; set; }

        [Display(Name = "Country")]
        [MaxLength(30, ErrorMessage ="Field {0} allows max {1} characters")]
        [Required(ErrorMessage = "Field {0} is required.")]
        public string Name { get; set; }

        public ICollection<State> States { get; set; }

        [Display(Name = "State Count")]
        public int StatesCount => States == null ? 0 : States.Count;

        [Display(Name = "Cities")]
        public int CitiesCount => States == null ? 0 : States.Sum(s => s.CitiesCount);

    }
}
