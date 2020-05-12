using System.Net;

namespace SalaryCounter.Service.Exception
{
    public class SalaryCounterGeneralException : SalaryCounterWebException
    {
        public SalaryCounterGeneralException(string message)
            : base(message, "General error", HttpStatusCode.BadRequest)
        {
        }
    }
}