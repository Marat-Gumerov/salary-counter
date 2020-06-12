using System;

namespace SalaryCounter.Model.Exception
{
    /// <summary> </summary>
    public class SalaryCounterModelException : ApplicationException
    {
        /// <summary> </summary>
        public bool ShouldBeLogged { get; }

        /// <summary> </summary>
        public SalaryCounterModelException(string message, bool shouldBeLogged = true) :
            base(message) => ShouldBeLogged = shouldBeLogged;
    }
}