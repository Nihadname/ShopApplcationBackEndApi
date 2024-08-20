namespace ShopApp.Mvc.ViewModels.ProductVM
{
    public class ProductListVM
    {
        
            public int Id { get; set; }
            public int Page { get; set; }
            public int TotalCount { get; set; }
            public List<ProductListItemVM> Items { get; set; }
        
        public class ProductListItemVM
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal SalePrice { get; set; }
            public decimal CurrentPrice { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime UpdatedDate { get; set; }
            public CategoryInProductVM Category { get; set; }
        }

        public class CategoryInProductVM
        {
            public string Name { get; set; }
            public int ProductsCount { get; set; }
        }
    }
}
