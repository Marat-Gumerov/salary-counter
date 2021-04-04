using System.Net;

namespace SalaryCounter.Service.Exception
{
    public class SalaryCounterInvalidInputException : SalaryCounterWebException
    {
        public SalaryCounterInvalidInputException(string message, bool shouldBeLogged = false) :
            base(message, "Invalid input",
                HttpStatusCode.BadRequest, shouldBeLogged)
        {
        }
    }
}
