using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shopping.Data.Entities
{
    public class City
    {
        public int Id { get; set; }

        [Display(Name = "City")]
        [MaxLength(50, ErrorMessage = "Field {0} allows max {1} characters")]
        [Required(ErrorMessage = "Field {0} is required.")]
        public string Name { get; set; }

        [JsonIgnore]
        public State State { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
