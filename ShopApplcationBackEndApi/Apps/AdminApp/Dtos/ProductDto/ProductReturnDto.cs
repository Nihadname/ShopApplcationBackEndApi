using ShopApplcationBackEndApi.Entities;

namespace ShopApplcationBackEndApi.Apps.AdminApp.Dtos.ProductDto
{
    public class ProductReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CostPrice { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public int ProfitMadeFromOne { get; set; }
        public CategoryInProductReturnDto Category { get; set; }
    }
    public class CategoryInProductReturnDto
    {
        public string Name { get; set; }
        public int ProductCount { get; set; }
    }
}
