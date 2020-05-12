using System.Net;

namespace SalaryCounter.Service.Exception
{
    public class SalaryCounterNotFoundException : SalaryCounterWebException
    {
        public SalaryCounterNotFoundException(string message) : base(message, "Not found",
            HttpStatusCode.NotFound)
        {
        }
    }
}