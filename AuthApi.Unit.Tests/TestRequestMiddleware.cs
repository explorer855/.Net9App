using AuthApi.Application.Middlewares;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace AuthApi.Tests.Infrastructure
{
    /// <summary>
    /// Tests for the Request-Culture Middleware
    /// </summary>
    public class TestRequestMiddleware
    {
        [Fact]
        public async Task RequestCultureMiddleware_DefaultTests()
        {
            //Create a new instance of the middleware
            var middleware = new RequestCultureMiddleware(
                next: (innerHttpContext) =>
                {
                    innerHttpContext.Response.WriteAsync(CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
                    return Task.CompletedTask;
                }
            );

            //Create the DefaultHttpContext
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            // Set Query string Culture parameter as DE
            context.Request.QueryString = context.Request.QueryString.Add("culture", "en-us");

            //Call the middleware
            await middleware.InvokeAsync(context);

            //Don't forget to rewind the stream
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var body = new StreamReader(context.Response.Body).ReadToEnd();

            //'en' seems OK for me as the default
            Assert.Equal("en", body);
        }

        [Fact]
        public async Task RequestCultureMiddleware_DE_Tests()
        {
            //Create a new instance of the middleware
            var middleware = new RequestCultureMiddleware(
                next: (innerHttpContext) =>
                {
                    innerHttpContext.Response.WriteAsync(CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
                    return Task.CompletedTask;
                }
            );

            //Create the DefaultHttpContext
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            // Set Query string Culture parameter as DE
            context.Request.QueryString = context.Request.QueryString.Add("culture", "de-DE");

            //Call the middleware
            await middleware.InvokeAsync(context);

            //Don't forget to rewind the stream
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var body = new StreamReader(context.Response.Body).ReadToEnd();

            //'en' seems OK for me as the default
            Assert.Equal("de", body);
        }
    }
}