using System.Net;

namespace SalaryCounter.Service.Exception
{
    public class SalaryCounterWebException : SalaryCounterException
    {
        public string ErrorType { get; }
        public HttpStatusCode StatusCode { get; }

        protected SalaryCounterWebException(string message, string errorType,
            HttpStatusCode statusCode)
            : base(message)
        {
            ErrorType = errorType;
            StatusCode = statusCode;
        }
    }
}