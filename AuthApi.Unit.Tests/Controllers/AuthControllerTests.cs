using AuthApi.Controllers;
using AuthApi.Infrastructure.Services;
using AuthApi.Models.Dtos;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AuthApi.Unit.Tests.Controllers;
public class AuthControllerTests
{
    private readonly Mock<IAuthService> _authServiceMock = new();
    private readonly Mock<IValidator<UserLoginRequest>> _validatorMock = new();
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _controller = new AuthController(_authServiceMock.Object);
    }

    [Fact]
    public async Task Login_ValidRequest_ReturnsOkWithAuthResponse()
    {
        // Arrange
        var userRequest = new UserLoginRequest { Username = "test@example.com", Password = "secure123" };
        var validationResult = new ValidationResult();

        _validatorMock.Setup(v => v.ValidateAsync(userRequest, default)).ReturnsAsync(validationResult);
        _authServiceMock.Setup(s => s.LoginAsync(userRequest)).ReturnsAsync(("Success", true));

        // Act
        var result = await _controller.Login(userRequest, _validatorMock.Object);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<AuthResponseDto>(okResult.Value);
        Assert.True(response.Success);
    }

    [Fact]
    public async Task Login_InvalidRequest_ThrowsValidationException()
    {
        // Arrange
        var userRequest = new UserLoginRequest();
        var failures = new List<ValidationFailure> { new("Email", "Email is required") };
        var validationResult = new ValidationResult(failures);

        _validatorMock.Setup(v => v.ValidateAsync(userRequest, default)).ReturnsAsync(validationResult);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ValidationException>(() => _controller.Login(userRequest, _validatorMock.Object));
        Assert.Equal("False", ex.Message);
        Assert.Single(ex.Errors);
    }
}