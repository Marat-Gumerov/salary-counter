using System;
using System.Net;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SalaryCounter.Model.Dto;
using SalaryCounter.Model.Exception;
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
                await ProcessException(httpContext, logger, exception);
            }
        }

        private static async Task ProcessException(HttpContext httpContext, ILogger<ExceptionMiddleware> logger,
            Exception exception)
        {
            var error = ToErrorDto(logger, exception);
            var response = httpContext.Response;
            response.StatusCode = (int)GetStatusCodeForException(exception);
            response.ContentType = "application/json";
            var errorJsonStream = error.ToJsonStream();
            var responseBodyStream = response.Body;
            await errorJsonStream.CopyToAsync(responseBodyStream);
        }

        private static ErrorDto ToErrorDto(ILogger logger, Exception exception) =>
            exception switch
            {
                SalaryCounterWebException webException => ProcessException(logger, webException,
                    webException.ErrorType, webException.ShouldBeLogged,
                    "Web exception that should never occur"),
                SalaryCounterException salaryCounterException => ProcessException(logger,
                    salaryCounterException, "unexpected", true,
                    "SalaryCounterException exception occured"),
                SalaryCounterModelException modelException => ProcessException(logger,
                    modelException, "model error", modelException.ShouldBeLogged,
                    "Model exception occured"),
                _ => ProcessException(logger, exception, "unexpected", true,
                    "Unexpected exception occured")
            };

        private static HttpStatusCode GetStatusCodeForException(Exception exception) =>
            exception switch
            {
                SalaryCounterWebException webException => webException.StatusCode,
                SalaryCounterException => HttpStatusCode.BadRequest,
                SalaryCounterModelException => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError
            };

        private static ErrorDto ProcessException(ILogger logger, Exception exception,
            string errorType, bool shouldBeLogged,
            string logMessage)
        {
            if (shouldBeLogged)
                // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
                logger.LogError(exception, logMessage);
            return new ErrorDto(exception.Message, errorType);
        }
    }
}