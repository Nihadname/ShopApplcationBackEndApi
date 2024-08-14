using AutoMapper;
using ShopApplcationBackEndApi.Apps.AdminApp.Dtos.CategoryDto;
using ShopApplcationBackEndApi.Apps.AdminApp.Dtos.ProductDto;
using ShopApplcationBackEndApi.Entities;

namespace ShopApplcationBackEndApi.Profiles
{
    public class MapperProfile:Profile
    {
            private readonly IHttpContextAccessor _contextAccessor;


        public MapperProfile(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;

            var uriBuilder  = new UriBuilder(_contextAccessor.HttpContext.Request.Scheme,
                _contextAccessor.HttpContext.Request.Host.Host
                ,_contextAccessor.HttpContext.Request.Host.Port.Value);
            var url=uriBuilder.Uri.AbsoluteUri;

            CreateMap<Category, CategoryReturnDto>()
                .ForMember(dest => dest.ImageUrl, map => map.MapFrom(src=>url+ "img/" + src.Image));
                                //.ForMember(dest => dest.ProductCount, map => map.MapFrom(src => src.Products.Count()));

            CreateMap<Product, ProductReturnDto>();
            CreateMap<Category,CategoryInProductReturnDto>();

        }
    }
}
