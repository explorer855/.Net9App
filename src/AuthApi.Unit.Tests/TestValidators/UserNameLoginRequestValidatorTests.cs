using IdentityApi.Application.Validators;
using IdentityApi.Models.Dtos;
using FluentValidation.TestHelper;

namespace IdentityApi.Tests.Validators;

public class UserNameLoginRequestValidatorTests
{
    private readonly UserNameLoginRequestValidator _validator;

    public UserNameLoginRequestValidatorTests()
    {
        _validator = new UserNameLoginRequestValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Username_Is_Empty()
    {
        var model = new UserNameLoginRequest { Username = "", Password = "Password123" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Username)
              .WithErrorMessage("Username is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Username_Too_Short()
    {
        var model = new UserNameLoginRequest { Username = "ab", Password = "Password123" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Username)
              .WithErrorMessage("Username must be between 3 and 50 characters long.");
    }

    [Fact]
    public void Should_Have_Error_When_Username_Too_Long()
    {
        var longUsername = new string('a', 51);
        var model = new UserNameLoginRequest { Username = longUsername, Password = "Password123" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Username)
              .WithErrorMessage("Username must be between 3 and 50 characters long.");
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Empty()
    {
        var model = new UserNameLoginRequest { Username = "validuser", Password = "" };
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
        var model = new UserNameLoginRequest { Username = "validuser", Password = password };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password)
              .WithErrorMessage("Password must be at least 6 characters long.");
    }

    [Fact]
    public void Should_Not_Have_Error_For_Valid_Model()
    {
        var model = new UserNameLoginRequest
        {
            Username = "validuser",
            Password = "Valid123"
        };

        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
