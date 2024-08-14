namespace ShopApplcationBackEndApi.Apps.AdminApp.Dtos.CategoryDto
{
    public class CategoryReturnDto
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public  DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string ImageUrl { get; set; }
        public int ProductsCount { get; set; }
    }
}
