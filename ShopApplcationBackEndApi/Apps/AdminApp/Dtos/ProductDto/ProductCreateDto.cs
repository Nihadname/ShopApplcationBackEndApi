﻿namespace ShopApplcationBackEndApi.Apps.AdminApp.Dtos.ProductDto
{
    public class ProductCreateDTO
    {
        public string Name { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CostPrice { get; set; }
        public int CategoryId { get; set; }
    }
}