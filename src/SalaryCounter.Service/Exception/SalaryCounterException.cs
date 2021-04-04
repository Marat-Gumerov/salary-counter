using System;

namespace SalaryCounter.Service.Exception
{
    public class SalaryCounterException : ApplicationException
    {
        protected SalaryCounterException(string message) : base(message)
        {
        }
    }
}
