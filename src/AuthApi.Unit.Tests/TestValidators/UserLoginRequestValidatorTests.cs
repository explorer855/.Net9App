using IdentityApi.Application.Validators;
using IdentityApi.Models.Dtos;
using FluentValidation.TestHelper;

namespace IdentityApi.Tests.Validators;

public class UserLoginRequestValidatorTests
{
    private readonly UserLoginRequestValidator _validator;

    public UserLoginRequestValidatorTests()
    {
        _validator = new UserLoginRequestValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Empty()
    {
        var model = new UserLoginRequest { Email = "", Password = "Password123" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email)
              .WithErrorMessage("Email is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Empty()
    {
        var model = new UserLoginRequest { Email = "test@example.com", Password = "" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password)
              .WithErrorMessage("Password is required.");
    }

    [Theory]
    [InlineData("nocaps123")]
    [InlineData("NOLOWER123")]
    [InlineData("NoDigits!")]
    public void Should_Have_Error_When_Password_Does_Not_Meet_Criteria(string password)
    {
        var model = new UserLoginRequest { Email = "test@example.com", Password = password };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password)
              .WithErrorMessage("Password must be at least 6 characters long.");
    }

    [Fact]
    public void Should_Not_Have_Error_For_Valid_Model()
    {
        var model = new UserLoginRequest
        {
            Email = "valid@example.com",
            Password = "Valid123"
        };

        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}