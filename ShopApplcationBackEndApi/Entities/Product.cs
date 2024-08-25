using ShopApplcationBackEndApi.Entities.Common;

namespace ShopApplcationBackEndApi.Entities
{
    public class Product:BaseEntity
    {
        public string Name { get; set; }
        public decimal SalePrice { get; set; }
        public  decimal CostPrice { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category    { get; set; }
        public ICollection<ProductImage>? productImages { get; set; }
    }
}
