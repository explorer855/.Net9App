using IdentityApi.Application.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace IdentityApi.Tests.Middlewares;
public class GlobalExceptionHandlerTests
{
    private readonly Mock<ILogger<GlobalExceptionHandler>> _mockLogger = new();
    private readonly Mock<IProblemDetailsService> _mockProblemDetailsService = new();
    private readonly GlobalExceptionHandler _handler;

    public GlobalExceptionHandlerTests()
    {
        _handler = new GlobalExceptionHandler(_mockLogger.Object, _mockProblemDetailsService.Object);
    }

    private static DefaultHttpContext CreateHttpContext(string method = "POST", string path = "/api/data")
    {
        var context = new DefaultHttpContext();
        context.Request.Method = method;
        context.Request.Path = path;
        return context;
    }
    private void SetupProblemDetailsService(bool result = true)
    {
        _mockProblemDetailsService
            .Setup(s => s.TryWriteAsync(It.IsAny<ProblemDetailsContext>()))
            .ReturnsAsync(result);
    }

    [Theory]
    [InlineData(typeof(NotImplementedException), StatusCodes.Status501NotImplemented)]
    [InlineData(typeof(NotSupportedException), StatusCodes.Status404NotFound)]
    [InlineData(typeof(InvalidOperationException), StatusCodes.Status409Conflict)]
    [InlineData(typeof(DbUpdateException), StatusCodes.Status409Conflict)]
    [InlineData(typeof(Exception), StatusCodes.Status500InternalServerError)]
    public async Task TryHandleAsync_SetsCorrectStatusCode(Type exceptionType, int expectedStatusCode)
    {
        // Arrange
        var exception = (Exception)Activator.CreateInstance(exceptionType)!;
        var context = CreateHttpContext();
        SetupProblemDetailsService();

        // Act
        var result = await _handler.TryHandleAsync(context, exception, CancellationToken.None);

        // Assert
        Assert.True(result);

        var resultException = exception.InnerException?.Message ?? exception.Message;

        _mockLogger.Verify(logger =>
            logger.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) =>
                    v.ToString().Contains(resultException)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);


        _mockProblemDetailsService.Verify(s => s.TryWriteAsync(It.IsAny<ProblemDetailsContext>()), Times.Once);
    }

    [Fact]
    public async Task TryHandleAsync_UsesInnerExceptionMessageInLogAndProblemDetails()
    {
        // Arrange
        var innerEx = new Exception("Inner boom!");
        var ex = new Exception("Outer shell", innerEx);
        var context = CreateHttpContext();
        SetupProblemDetailsService();

        // Act
        await _handler.TryHandleAsync(context, ex, CancellationToken.None);

        // Assert
        _mockLogger.Verify(
               x => x.Log(
                   LogLevel.Error,
                   It.IsAny<EventId>(),
                   It.Is<It.IsAnyType>((v, t) =>
                        v.ToString().Contains("Inner boom!")),
                   null,
                   It.IsAny<Func<It.IsAnyType, Exception, string>>()),
               Times.Once);

        _mockProblemDetailsService.Verify(s => s.TryWriteAsync(It.IsAny<ProblemDetailsContext>()), Times.Once);
    }
}