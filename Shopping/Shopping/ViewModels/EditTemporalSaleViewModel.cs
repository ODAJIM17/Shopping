using System.ComponentModel.DataAnnotations;

namespace Shopping.ViewModels
{
    public class EditTemporalSaleViewModel
    {
        public int Id { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comments")]
        public string Remarks { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Qty")]
        [Range(0.0000001, float.MaxValue, ErrorMessage = "Quantity must be grater than 0.")]
        [Required(ErrorMessage = "{0} is required.")]
        public float Quantity { get; set; }
    }
}
