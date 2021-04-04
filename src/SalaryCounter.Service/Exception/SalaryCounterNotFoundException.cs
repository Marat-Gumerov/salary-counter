using System.Net;

namespace SalaryCounter.Service.Exception
{
    public class SalaryCounterNotFoundException : SalaryCounterWebException
    {
        public SalaryCounterNotFoundException(string message, bool shouldBeLogged = false) : base(
            message, "Not found",
            HttpStatusCode.NotFound, shouldBeLogged)
        {
        }
    }
}
