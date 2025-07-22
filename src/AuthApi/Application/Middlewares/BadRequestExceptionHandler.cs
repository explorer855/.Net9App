using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Application.Middlewares
{
    public sealed class BadRequestExceptionHandler(ILogger<BadRequestExceptionHandler> logger,
        IProblemDetailsService problemDetailsService)
        : IExceptionHandler
    {
        private readonly IProblemDetailsService _problemDetailsService = problemDetailsService ?? throw new ArgumentNullException(nameof(problemDetailsService));

        /// <summary>
        /// Handle the exception and return a ProblemDetails response if applicable.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="exception"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is ValidationException validationEx)
            {
                logger.LogError(exception.InnerException?.Message ?? exception.Message);

                var statusCode = StatusCodes.Status400BadRequest;

                httpContext.Response.StatusCode = statusCode;

                var problemDetails = new ProblemDetails
                {
                    Title = "An exception occurred!",
                    Detail = exception.Message,
                    Type = exception.GetType().Name,
                    Status = statusCode,
                    Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                    Extensions = new Dictionary<string, object?>
                    {
                        ["traceId"] = httpContext.TraceIdentifier,
                        ["requestId"] = httpContext.Request.Headers["Request-Id"].ToString(),
                        ["errors"] = validationEx.Errors.Select(x => x.ErrorMessage)
                    }
                };

                return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
                {
                    HttpContext = httpContext,
                    ProblemDetails = problemDetails,
                    Exception = exception,
                });
            }
            else 
            {
                return false;
            }            
        }
    }
}
