using System.ComponentModel.DataAnnotations;

namespace Shopping.Data.Entities
{
    public class TemporalSale
    {
        public int Id { get; set; }

        public User User { get; set; }

        public Product Product { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Qty")]
        [Required(ErrorMessage = "{0} is required.")]
        public float Quantity { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Coments")]
        public string? Remarks { get; set; }

    }
}
