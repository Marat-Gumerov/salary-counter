using System.Net;

namespace SalaryCounter.Service.Exception
{
    public class SalaryCounterWebException : SalaryCounterException
    {
        public string ErrorType { get; }
        public HttpStatusCode StatusCode { get; }
        public bool ShouldBeLogged { get; }

        protected SalaryCounterWebException(string message, string errorType,
            HttpStatusCode statusCode, bool shouldBeLogged)
            : base(message)
        {
            ErrorType = errorType;
            StatusCode = statusCode;
            ShouldBeLogged = shouldBeLogged;
        }
    }
}