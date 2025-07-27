using IdentityApi.Application.Middlewares;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace IdentityApi.Tests.Middlewares
{
    public class BadRequestExceptionHandlerTests
    {
        private readonly Mock<ILogger<BadRequestExceptionHandler>> _loggerMock;
        private readonly Mock<IProblemDetailsService> _problemDetailsServiceMock;
        private readonly BadRequestExceptionHandler _handler;

        public BadRequestExceptionHandlerTests()
        {
            _loggerMock = new Mock<ILogger<BadRequestExceptionHandler>>();
            _problemDetailsServiceMock = new Mock<IProblemDetailsService>();
            _handler = new BadRequestExceptionHandler(_loggerMock.Object, _problemDetailsServiceMock.Object);
        }

        [Fact]
        public async Task TryHandleAsync_ReturnsTrue_WhenValidationExceptionOccurs()
        {
            // Arrange
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("Name", "Name is required"),
                new ValidationFailure("Email", "Email is invalid")
            };

            var validationException = new ValidationException(failures);

            var context = new DefaultHttpContext();
            context.TraceIdentifier = "trace-123";
            context.Request.Method = "POST";
            context.Request.Path = "/api/test";
            context.Request.Headers["Request-Id"] = "req-456";

            _problemDetailsServiceMock
                .Setup(x => x.TryWriteAsync(It.IsAny<ProblemDetailsContext>()))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.TryHandleAsync(context, validationException, CancellationToken.None);

            // Assert
            Assert.True(result);
            Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);

            _problemDetailsServiceMock.Verify(x =>
                x.TryWriteAsync(It.Is<ProblemDetailsContext>(ctx =>
                    ctx.ProblemDetails.Detail == validationException.Message &&
                    ctx.ProblemDetails.Status == StatusCodes.Status400BadRequest &&
                    ctx.ProblemDetails.Extensions.ContainsKey("errors") &&
                    ctx.ProblemDetails.Extensions.ContainsKey("traceId") &&
                    ctx.ProblemDetails.Extensions.ContainsKey("requestId")
                )), Times.Once);
        }

        [Fact]
        public async Task TryHandleAsync_ReturnsFalse_WhenExceptionIsNotValidationException()
        {
            // Arrange
            var ex = new InvalidOperationException("Something went wrong");
            var context = new DefaultHttpContext();

            // Act
            var result = await _handler.TryHandleAsync(context, ex, CancellationToken.None);

            // Assert
            Assert.False(result);
            _problemDetailsServiceMock.Verify(x => x.TryWriteAsync(It.IsAny<ProblemDetailsContext>()), Times.Never);
        }
    }
}
