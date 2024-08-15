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

            RuleFor(s => s).Custom((c, context) =>
            {
                if(!(c.Photo != null && c.Photo.ContentType.Contains("image/")))
                {
                    context.AddFailure("Photo", "only image is accepted");
                }
                if (!(c.Photo != null && c.Photo.Length/10124>6000))
                {
                    context.AddFailure("Photo", "data stroage more than expected");
                }
            });
        }
    }
}
