using AuthApi.Data;
using AuthApi.Infrastructure.Services;
using AuthApi.Models.Dtos;
using AuthApi.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AuthApi.Unit.Tests.Services;

/// <summary>
/// AuthService unit tests
/// </summary>
public class AuthServiceTests
{
    private readonly Mock<AuthDbContext> _dbContextMock;
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            userStoreMock.Object, null, null, null, null, null, null, null, null);

        var roleStoreMock = new Mock<IRoleStore<IdentityRole>>();
        _roleManagerMock = new Mock<RoleManager<IdentityRole>>(
            roleStoreMock.Object, null, null, null, null);

        var options = new DbContextOptionsBuilder<AuthDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _dbContextMock = new Mock<AuthDbContext>(options);

        _authService = new AuthService(_userManagerMock.Object);
    }

    [Fact]
    public async Task LoginAsync_ReturnsUserId_WhenCredentialsAreValid()
    {
        // Arrange
        var user = new ApplicationUser { Id = "user123", UserName = "testuser" };
        var userLogin = new UserLoginRequest { Username = "testuser", Password = "password" };

        var users = new[] { user }.AsQueryable();
        var dbSetMock = new Mock<DbSet<ApplicationUser>>();
        dbSetMock.As<IQueryable<ApplicationUser>>().Setup(m => m.Provider).Returns(users.Provider);
        dbSetMock.As<IQueryable<ApplicationUser>>().Setup(m => m.Expression).Returns(users.Expression);
        dbSetMock.As<IQueryable<ApplicationUser>>().Setup(m => m.ElementType).Returns(users.ElementType);
        dbSetMock.As<IQueryable<ApplicationUser>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

        _dbContextMock.Setup(db => db.ApplicationUsers).Returns(dbSetMock.Object);
        _userManagerMock.Setup(um => um.FindByNameAsync(user.UserName)).ReturnsAsync(user);
        _userManagerMock.Setup(um => um.CheckPasswordAsync(user, userLogin.Password)).ReturnsAsync(true);

        // Act
        var (result, success) = await _authService.LoginAsync(userLogin);

        // Assert
        Assert.True(success);
        Assert.Equal("user123", result);
    }

    [Fact]
    public async Task LoginAsync_ReturnsError_WhenCredentialsAreInvalid()
    {
        // Arrange
        var user = new ApplicationUser { Id = "user123", UserName = "testuser" };
        var userLogin = new UserLoginRequest { Username = "testuser", Password = "wrongpassword" };

        var users = new[] { user }.AsQueryable();
        var dbSetMock = new Mock<DbSet<ApplicationUser>>();
        dbSetMock.As<IQueryable<ApplicationUser>>().Setup(m => m.Provider).Returns(users.Provider);
        dbSetMock.As<IQueryable<ApplicationUser>>().Setup(m => m.Expression).Returns(users.Expression);
        dbSetMock.As<IQueryable<ApplicationUser>>().Setup(m => m.ElementType).Returns(users.ElementType);
        dbSetMock.As<IQueryable<ApplicationUser>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

        _dbContextMock.Setup(db => db.ApplicationUsers).Returns(dbSetMock.Object);
        _userManagerMock.Setup(um => um.FindByNameAsync(user.UserName)).ReturnsAsync(user);
        _userManagerMock.Setup(um => um.CheckPasswordAsync(user, userLogin.Password)).ReturnsAsync(false);

        // Act
        var (result, success) = await _authService.LoginAsync(userLogin);

        // Assert
        Assert.False(success);
        Assert.Equal("Incorrect Username/Password!", result);
    }

    [Fact]
    public async Task RegisterAsync_ReturnsSuccessMessage_WhenRegistrationSucceeds()
    {
        // Arrange
        var registerUser = new RegisterUserRequest
        {
            Username = "newuser",
            Email = "newuser@example.com",
            PhoneNumber = "1234567890",
            Password = "Password123!"
        };

        _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), registerUser.Password))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _authService.RegisterAsync(registerUser);

        // Assert
        Assert.Equal("User registered successfully", result);
    }

    [Fact]
    public async Task RegisterAsync_ReturnsErrorMessages_WhenRegistrationFails()
    {
        // Arrange
        var registerUser = new RegisterUserRequest
        {
            Username = "failuser",
            Email = "failuser@example.com",
            PhoneNumber = "1234567890",
            Password = "Password123!"
        };

        var identityError = new IdentityError { Description = "Email is already taken" };
        var failedResult = IdentityResult.Failed(identityError);

        _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), registerUser.Password))
            .ReturnsAsync(failedResult);

        // Act
        var result = await _authService.RegisterAsync(registerUser);

        // Assert
        Assert.Contains("Email is already taken", result);
    }
}