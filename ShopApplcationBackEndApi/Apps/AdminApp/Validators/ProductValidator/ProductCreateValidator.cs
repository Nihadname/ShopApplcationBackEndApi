using FluentValidation;
using ShopApplcationBackEndApi.Apps.AdminApp.Dtos.ProductDto;

namespace ShopApplcationBackEndApi.Apps.AdminApp.Validators.ProductValidator
{
    public class ProductCreateValidator:AbstractValidator<ProductCreateDTO>
    {
        public ProductCreateValidator()
        {
            RuleFor(s => s.Name).NotEmpty().MaximumLength(40);
        }
    }
}
