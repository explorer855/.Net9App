using AuthApi.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Cryptography;
using System.Text.Json;

namespace AuthApi.Tests.Infrastructure
{
    /// <summary>
    /// Global Exception Handler Tests
    /// </summary>
    public class GlobalExceptionHandlerTests
    {
        private readonly Mock<ILogger<GlobalExceptionHandler>> _loggerMock;
        private readonly GlobalExceptionHandler _handler;

        public GlobalExceptionHandlerTests()
        {
            _loggerMock = new Mock<ILogger<GlobalExceptionHandler>>();
            _handler = new GlobalExceptionHandler(_loggerMock.Object);
        }

        private static HttpContext CreateHttpContext()
        {
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            return context;
        }

        private async Task<ProblemDetails> GetProblemDetailsFromResponse(HttpContext context)
        {
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            return await JsonSerializer.DeserializeAsync<ProblemDetails>(
                context.Response.Body,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        [Theory]
        [InlineData(typeof(NotImplementedException), StatusCodes.Status501NotImplemented)]
        [InlineData(typeof(NotSupportedException), StatusCodes.Status404NotFound)]
        [InlineData(typeof(InvalidOperationException), StatusCodes.Status409Conflict)]
        [InlineData(typeof(DbUpdateConcurrencyException), StatusCodes.Status409Conflict)]
        [InlineData(typeof(CryptographicException), StatusCodes.Status409Conflict)]
        [InlineData(typeof(DbUpdateException), StatusCodes.Status409Conflict)]
        [InlineData(typeof(Exception), StatusCodes.Status500InternalServerError)]
        public async Task TryHandleAsync_SetsCorrectStatusCodeAndReturnsProblemDetails(Type exceptionType, int expectedStatusCode)
        {
            // Arrange
            var context = CreateHttpContext();
            var innerException = new Exception("Inner error message") { HelpLink = "help-link" };
            var exception = (Exception)Activator.CreateInstance(exceptionType, "Test error");
            exception = exceptionType == typeof(Exception) ? exception : (Exception)Activator.CreateInstance(exceptionType, "Test error", innerException);

            // Act
            var result = await _handler.TryHandleAsync(context, exception, CancellationToken.None);

            // Assert
            Assert.True(result);
            Assert.Equal(expectedStatusCode, context.Response.StatusCode);

            var problemDetails = await GetProblemDetailsFromResponse(context);
            Assert.Equal("An exception occurred!!", problemDetails.Title);
            Assert.Equal(expectedStatusCode, problemDetails.Status);
            if (exception.InnerException != null)
                Assert.Equal(exception.InnerException.Message, problemDetails.Detail);
            else
                Assert.Null(problemDetails.Detail);
            Assert.Equal(exception.HelpLink, problemDetails.Instance);
        }

        [Fact]
        public async Task TryHandleAsync_LogsError()
        {
            // Arrange
            var context = CreateHttpContext();
            var innerException = new Exception("Inner error message");
            var exception = new InvalidOperationException("Test error", innerException);

            // Act
            await _handler.TryHandleAsync(context, exception, CancellationToken.None);

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Inner error message")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
