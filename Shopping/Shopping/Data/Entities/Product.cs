using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        [MaxLength(50, ErrorMessage = "Field {0} allows max {1} characters")]
        [Required(ErrorMessage = "{0} is required.")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        [MaxLength(500, ErrorMessage = "Field {0} allows max {1} characters")]
        public string Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Price")]
        [Required(ErrorMessage = "{0} is required.")]
        public decimal Price { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Inventory")]
        [Required(ErrorMessage = "{0} is required.")]
        public float Stock { get; set; }

        public ICollection<ProductCategory> ProductCategories { get; set; }

        [Display(Name = "Categories")]
        public int CategoriesNumber => ProductCategories == null ? 0 : ProductCategories.Count;

        public ICollection<ProductImage> ProductImages { get; set; }

        [Display(Name = "Photos")]
        public int ImagesNumber => ProductImages == null ? 0 : ProductImages.Count;

        //TODO: Pending to change to the correct path
        [Display(Name = "Photo")]
        public string ImageFullPath => ProductImages == null || ProductImages.Count == 0
            ? $"https://localhost:7270/images/noimage.png"
            : ProductImages.FirstOrDefault().ImageFullPath;
    }
}
