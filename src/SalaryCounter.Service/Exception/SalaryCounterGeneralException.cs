using System.Net;

namespace SalaryCounter.Service.Exception
{
    public class SalaryCounterGeneralException : SalaryCounterWebException
    {

        public SalaryCounterGeneralException(string message, bool shouldBeLogged = false)
            : base(message, "General error", HttpStatusCode.BadRequest, shouldBeLogged)
        {
        }
    }
}
