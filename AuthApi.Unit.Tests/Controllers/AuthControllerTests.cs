using AuthApi.Controllers;
using AuthApi.Infrastructure.Services;
using AuthApi.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AuthApi.Unit.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _authServiceMock = new Mock<IAuthService>();
            _controller = new AuthController(_authServiceMock.Object);
        }

        [Fact]
        public async Task Login_NullUser_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _controller.Login(null));
        }


        [Fact]
        public async Task Login_ReturnsOk_WithAuthResponseDto()
        {
            // Arrange
            var loginRequest = new UserLoginRequest { Username = "testuser", Password = "password" };
            var expectedTuple = ("user-id-123", true);
            _authServiceMock.Setup(s => s.LoginAsync(loginRequest)).ReturnsAsync(expectedTuple);

            // Act
            var result = await _controller.Login(loginRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<AuthResponseDto>(okResult.Value);
            Assert.Equal(expectedTuple.Item1, dto.TResponse);
            Assert.Equal(expectedTuple.Item2, dto.Success);
        }

        [Fact]
        public async Task Register_ReturnsOk_WithResult()
        {
            // Arrange
            var registerRequest = new RegisterUserRequest
            {
                Email = "test@example.com",
                Name = "Test User",
                Password = "password",
                PhoneNumber = "1234567890",
                Username = "testuser"
            };
            var expectedResult = "registered-user-id";
            _authServiceMock.Setup(s => s.RegisterAsync(registerRequest)).ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Register(registerRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var dtoResult = Assert.IsType<AuthResponseDto>(okResult.Value);
            Assert.Equal(expectedResult, dtoResult.TResponse);
        }
    }
}
