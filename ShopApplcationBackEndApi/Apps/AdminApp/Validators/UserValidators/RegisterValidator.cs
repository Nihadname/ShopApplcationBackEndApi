using FluentValidation;
using ShopApplcationBackEndApi.Apps.AdminApp.Dtos.UserDto;

namespace ShopApplcationBackEndApi.Apps.AdminApp.Validators.UserValidators
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(s => s.UserName).NotEmpty().WithMessage("not empty")
                .MaximumLength(100).WithMessage("max is 100");

            RuleFor(s => s.FullName).NotEmpty().WithMessage("not empty")
                .MaximumLength(150).WithMessage("max is 150");

            RuleFor(s => s.Email).NotEmpty().WithMessage("not empty")
                .MaximumLength(100).WithMessage("max is 100")
                .EmailAddress().WithMessage("should be in email format");
            RuleFor(s => s.Password).NotEmpty().WithMessage("not empty")
                .MinimumLength(8)
                .MaximumLength(100).WithMessage("max is 100");
            RuleFor(s => s.RepeatPassword).NotEmpty().WithMessage("not empty")
                                .MinimumLength(8)
           .MaximumLength(100).WithMessage("max is 100");
            RuleFor(s => s).Custom((s, context) =>
            {
                if (s.Password != s.RepeatPassword)
                {
                    context.AddFailure("Password", "paswords dont match in this part");
                }

            });

        }
    }
}
