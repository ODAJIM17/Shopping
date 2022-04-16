﻿using System.ComponentModel.DataAnnotations;

namespace Shopping.ViewModels
{
    public class EditProductViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        [MaxLength(50, ErrorMessage = "{0} Max allowed {1} characters.")]
        [Required(ErrorMessage = "{0} is required.")]
        public string Name { get; set; }

        [Display(Name = "Descripcion")]
        [MaxLength(500, ErrorMessage = "{0} Max allowed {1} characters.")]
        [Required(ErrorMessage = "{0} is required.")]
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Price")]
        [Required(ErrorMessage = "{0} is required.")]
        public decimal Price { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Inventory")]
        [Required(ErrorMessage = "{0} is required.")]
        public float Stock { get; set; }
    }
}
