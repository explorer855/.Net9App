using AuthApi.Infrastructure.Services;
using AuthApi.Models.Dtos;
using AuthApi.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace AuthApi.Tests.Infrastructure.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<SignInManager<ApplicationUser>> _signInManagerMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();

            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            var contextAccessorMock = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
            var userClaimsPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();

            _signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
                _userManagerMock.Object,
                contextAccessorMock.Object,
                userClaimsPrincipalFactoryMock.Object,
                null, null, null, null);

            _authService = new AuthService(_userManagerMock.Object, _signInManagerMock.Object);
        }

        [Fact]
        public async Task LoginAsync_ValidCredentials_ReturnsUserIdAndSuccess()
        {
            // Arrange
            var userLogin = new UserLoginRequest { Email = "test@example.com", Password = "Password123!" };
            var appUser = new ApplicationUser { Id = "123", UserName = "testuser", Email = userLogin.Email };

            _userManagerMock.Setup(um => um.FindByEmailAsync(userLogin.Email)).ReturnsAsync(appUser);
            _signInManagerMock.Setup(sm => sm.PasswordSignInAsync(appUser.UserName, userLogin.Password, false, true))
                .ReturnsAsync(SignInResult.Success);

            // Act
            var result = await _authService.LoginAsync(userLogin);

            // Assert
            Assert.True(result.Item2);
            Assert.Equal("123", result.Item1);
        }

        [Fact]
        public async Task LoginAsync_InvalidCredentials_ReturnsErrorMessageAndFailure()
        {
            // Arrange
            var userLogin = new UserLoginRequest { Email = "test@example.com", Password = "wrongpassword" };
            var appUser = new ApplicationUser { Id = "123", UserName = "testuser", Email = userLogin.Email };

            _userManagerMock.Setup(um => um.FindByEmailAsync(userLogin.Email)).ReturnsAsync(appUser);
            _signInManagerMock.Setup(sm => sm.PasswordSignInAsync(appUser.UserName, userLogin.Password, false, true))
                .ReturnsAsync(SignInResult.Failed);

            // Act
            var result = await _authService.LoginAsync(userLogin);

            // Assert
            Assert.False(result.Item2);
            Assert.Equal("Incorrect Username/Password!", result.Item1);
        }

        [Fact]
        public async Task RegisterAsync_SuccessfulRegistration_ReturnsSuccessMessage()
        {
            // Arrange
            var request = new RegisterUserRequest
            {
                Username = "newuser",
                Email = "newuser@example.com",
                Password = "StrongPassword123!",
                PhoneNumber = "1234567890"
            };

            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), request.Password))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _authService.RegisterAsync(request);

            // Assert
            Assert.Equal("User registered successfully", result);
        }

        [Fact]
        public async Task RegisterAsync_FailedRegistration_ReturnsErrorMessages()
        {
            // Arrange
            var request = new RegisterUserRequest
            {
                Username = "baduser",
                Email = "baduser@example.com",
                Password = "weak",
                PhoneNumber = "1234567890"
            };

            var identityErrors = new[]
            {
                new IdentityError { Description = "Password is too weak." },
                new IdentityError { Description = "Email already exists." }
            };

            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), request.Password))
                .ReturnsAsync(IdentityResult.Failed(identityErrors));

            // Act
            var result = await _authService.RegisterAsync(request);

            // Assert
            Assert.Contains("Password is too weak.", result);
            Assert.Contains("Email already exists.", result);
        }
    }
}
