using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace AuthApi.Application.Middlewares
{
    public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger,
        IProblemDetailsService problemDetailsService)
        : IExceptionHandler
    {
        private readonly IProblemDetailsService _problemDetailsService = problemDetailsService ?? throw new ArgumentNullException(nameof(problemDetailsService));

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception.InnerException?.Message ?? exception.Message);

            httpContext.Response.StatusCode = exception switch
            {
                NotImplementedException _ => StatusCodes.Status501NotImplemented,
                NotSupportedException _ => StatusCodes.Status404NotFound,
                InvalidOperationException _ or DbUpdateConcurrencyException _ or CryptographicException _ or DbUpdateException _ => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError,
            };

            var problemDetails = new ProblemDetails
            {
                Title = "An exception occurred!",
                Status = httpContext.Response.StatusCode,
                Detail = exception.InnerException?.Message,
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
            };

            return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = problemDetails,
                Exception = exception,
            });
        }
    }
}
