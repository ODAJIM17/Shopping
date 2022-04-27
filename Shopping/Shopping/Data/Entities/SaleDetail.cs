using System.ComponentModel.DataAnnotations;

namespace Shopping.Data.Entities
{
    public class SaleDetail
    {
        public int Id { get; set; }

        public Sale Sale { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comments")]
        public string Remarks { get; set; }

        public Product Product { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Qty")]
        [Required(ErrorMessage = "{0} is required.")]
        public float Quantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Total")]
        public decimal Value => Product == null ? 0 : (decimal)Quantity * Product.Price;
    }
}
