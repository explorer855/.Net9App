using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;
using System.Security.Cryptography;

namespace AuthApi.Application.Middlewares
{
    public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception.InnerException?.Message);

            httpContext.Response.StatusCode = exception switch
            {
                NotImplementedException _ => StatusCodes.Status501NotImplemented,
                NotSupportedException _ => StatusCodes.Status404NotFound,
                InvalidOperationException _ or DbUpdateConcurrencyException _ or CryptographicException _ or DbUpdateException _ => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError,
            };

            var problemDetails = new ProblemDetails
            {
                Title = "An exception occurred!!",
                Status = httpContext.Response.StatusCode,
                Detail = exception.InnerException?.Message,
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
            };

            await httpContext.Response.WriteAsJsonAsync(value: problemDetails, options: null, contentType: MediaTypeNames.Application.ProblemJson, cancellationToken);
            return true;
        }
    }
}
