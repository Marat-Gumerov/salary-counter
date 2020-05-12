using System.Net;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SalaryCounter.Api.Extension;
using SalaryCounter.Api.Model;
using SalaryCounter.Service.Exception;

namespace SalaryCounter.Api.Middleware
{
    [UsedImplicitly]
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        [UsedImplicitly]
        public async Task Invoke(HttpContext httpContext, ILogger<ExceptionMiddleware> logger)
        {
            try
            {
                await next(httpContext);
            }
            catch (SalaryCounterWebException exception)
            {
                await ProcessException(httpContext, exception.Message, exception.StatusCode, exception.ErrorType);
            }
            catch (SalaryCounterException exception)
            {
                logger.LogError(exception, "SalaryCounterException occured");
                await ProcessException(httpContext, exception.Message, HttpStatusCode.BadRequest, "unexpected");
            }
        }

        private static async Task ProcessException(HttpContext httpContext,
            string message, HttpStatusCode statusCode, string errorType)
        {
            httpContext.Response.StatusCode = (int) statusCode;
            httpContext.Response.ContentType = "application/json";
            var body = httpContext.Response.Body;
            var error = new ErrorDto(message, errorType).ToStream();
            await error.CopyToAsync(body);
        }
    }
}