using ShopApplcationBackEndApi.Entities.Common;

namespace ShopApplcationBackEndApi.Entities
{
    public class Product:BaseEntity
    {
        public string Name { get; set; }
        public decimal SalePrice { get; set; }
        public  decimal CostPrice { get; set; }
        public bool IsDeleted { get; set; }
    }
}
