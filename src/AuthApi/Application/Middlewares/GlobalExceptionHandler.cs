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

            var problemDetails = new ProblemDetails
            {
                Title = "An exception occurred!",
                Detail = exception.Message,
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                Type = exception.GetType().Name,
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
