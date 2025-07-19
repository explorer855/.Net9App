using AuthApi.Models.Dtos;
using FluentValidation;

namespace AuthApi.Application.Validators
{
    public class UserLoginRequestValidator
        : AbstractValidator<UserLoginRequest>
    {
        public UserLoginRequestValidator()
        {
            RuleFor(x => x.Username)
                .Cascade(cascadeMode: CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Username is required.")
                .Length(3, 50)
                .WithMessage("Username must be between 3 and 50 characters long.");

            RuleFor(x => x.Password)
                .Cascade(cascadeMode: CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(6)
                .Must(password => password.Any(char.IsUpper)
                    && password.Any(char.IsLower)
                    && password.Any(char.IsDigit)
                    && password.Any(char.IsAscii))
                .WithMessage("Password must be at least 6 characters long.");
        }
    }
}
