﻿namespace Shopping.ViewModels
{
    public class HomeViewModel
    {
        public ICollection<ProductsHomeViewModel> Products { get; set; }

        public float Quantity { get; set; }

    }
}
