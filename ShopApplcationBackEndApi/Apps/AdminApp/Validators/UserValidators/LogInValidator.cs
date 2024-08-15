using FluentValidation;
using ShopApplcationBackEndApi.Apps.AdminApp.Dtos.UserDto;

namespace ShopApplcationBackEndApi.Apps.AdminApp.Validators.UserValidators
{
    public class LogInValidator : AbstractValidator<LoginDto>
    {
        public LogInValidator()
        {
            RuleFor(s => s.UserName).NotEmpty().WithMessage("not empty")
              .MaximumLength(100).WithMessage("max is 100");
            RuleFor(s => s.Password).NotEmpty().WithMessage("not empty")
               .MinimumLength(8)
               .MaximumLength(100).WithMessage("max is 100");
        }
    }
}
