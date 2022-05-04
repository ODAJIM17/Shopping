using System.ComponentModel.DataAnnotations;

namespace Shopping.Data.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Display(Name = "Categories")]
        [MaxLength(30, ErrorMessage = "Field {0} allows max {1} characters")]
        [Required(ErrorMessage = "Field {0} is required.")]
        public string Name { get; set; }

        public ICollection<ProductCategory> ProductCategories { get; set; }

        [Display(Name = "# Productos")]
        public int ProductsNumber => ProductCategories == null ? 0 : ProductCategories.Count();

    }
}
