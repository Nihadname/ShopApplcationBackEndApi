using AutoMapper;
using ShopApplcationBackEndApi.Apps.AdminApp.Dtos.CategoryDto;
using ShopApplcationBackEndApi.Entities;

namespace ShopApplcationBackEndApi.Profiles
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<Category, CategoryReturnDto>();

            CreateMap<CategoryCreateDto, Category>();
            CreateMap<CategoryUpdateDto, Category>();

        }
    }
}
