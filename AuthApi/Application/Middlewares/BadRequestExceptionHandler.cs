using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace AuthApi.Application.Middlewares
{
    public class BadRequestExceptionHandler(ILogger<BadRequestExceptionHandler> logger)
        : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception.InnerException?.Message);

            var validationException = exception as ValidationException;

            httpContext.Response.StatusCode = exception switch
            {
                ValidationException _ => StatusCodes.Status400BadRequest,
                _ => throw new NotImplementedException()
            };

            var problemDetails = new ProblemDetails
            {
                Title = "An exception occurred due to BadRequest!!",
                Status = httpContext.Response.StatusCode,
                Detail = validationException.InnerException?.Message,
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                Extensions = new Dictionary<string, object?>
                {
                    { "errors", validationException.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }) }
                }
            };

            await httpContext.Response.WriteAsJsonAsync(value: problemDetails, options: null, contentType: MediaTypeNames.Application.ProblemJson, cancellationToken);
            return true;
        }
    }
}
