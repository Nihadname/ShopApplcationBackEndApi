using ShopApplcationBackEndApi.Entities;

namespace ShopApplcationBackEndApi.Apps.AdminApp.Dtos.CategoryDto
{
    public class categoryListDto
    {
        public int Page { get; set; }
        public int TotalCount { get; set; }
        public List<CategoryListItemDto> categories { get; set; }
    }
}
