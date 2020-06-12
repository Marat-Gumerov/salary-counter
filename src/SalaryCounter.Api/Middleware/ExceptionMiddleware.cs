using System;
using System.Net;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SalaryCounter.Model.Dto;
using SalaryCounter.Model.Extension;
using SalaryCounter.Service.Exception;

namespace SalaryCounter.Api.Middleware
{
    [UsedImplicitly]
    internal class ExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next) => this.next = next;

        [UsedImplicitly]
        public async Task Invoke(HttpContext httpContext, ILogger<ExceptionMiddleware> logger)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception exception)
            {
                var error = exception switch
                {
                    SalaryCounterWebException webException => ProcessException(logger, webException,
                        webException.ErrorType, webException.ShouldBeLogged,
                        "Web exception that should never occur"),
                    SalaryCounterException salaryCounterException => ProcessException(logger,
                        salaryCounterException, "unexpected", true,
                        "SalaryCounterException exception occured"),
                    _ => ProcessException(logger, exception, "unexpected", true,
                        "Unexpected exception occured")
                };
                httpContext.Response.StatusCode = (int) GetStatusCodeForException(exception);
                httpContext.Response.ContentType = "application/json";
                await error.ToJsonStream().CopyToAsync(httpContext.Response.Body);
            }
        }

        private static HttpStatusCode GetStatusCodeForException(Exception exception)
        {
            return exception switch
            {
                SalaryCounterWebException webException => webException.StatusCode,
                SalaryCounterException _ => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError
            };
        }

        private static ErrorDto ProcessException(ILogger logger, Exception exception,
            string errorType, bool shouldBeLogged,
            string logMessage)
        {
            if (shouldBeLogged)
                logger.LogError(exception, logMessage);
            var error = new ErrorDto(exception.Message, errorType);
            return error;
        }
    }
}