using Shopping.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Shopping.ViewModels
{
    public class AddProductToCartViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        [MaxLength(50, ErrorMessage = "{0} Allows max {1} characters.")]
        [Required(ErrorMessage = "{0} is required.")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        [MaxLength(500, ErrorMessage = "{0} Allows max {1} characters.")]
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Price")]
        [Required(ErrorMessage = "{0} is required.")]
        public decimal Price { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "In Stock")]
        [Required(ErrorMessage = "{0} is required.")]
        public float Stock { get; set; }

        [Display(Name = "Categories")]
        public string Categories { get; set; }

        public ICollection<ProductImage> ProductImages { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Qty")]
        [Range(0.0000001, float.MaxValue, ErrorMessage = "Qty must be greater than 0.")]
        [Required(ErrorMessage = "{0} is required.")]
        public float Quantity { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comments")]
        public string Remarks { get; set; }

    }
}
