using FluentValidation;
using ShopApplcationBackEndApi.Apps.AdminApp.Dtos.CategoryDto;

namespace ShopApplcationBackEndApi.Apps.AdminApp.Validators.CategoryValidators
{
    public class CategoryCreateValidator : AbstractValidator<CategoryCreateDto>
    {
        public CategoryCreateValidator()
        {
            RuleFor(s => s.Name).NotEmpty().WithMessage("not empty").MaximumLength(40)
                   .WithMessage("max length is 40 ");
        }
    }
}
