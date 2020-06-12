using System;

namespace SalaryCounter.Model.Extension
{
    /// <summary>
    ///     DateTime extension methods
    /// </summary>
    public static class DateTimeExtension
    {
        /// <summary>
        ///     Finds difference between dates in full years
        /// </summary>
        public static int GetYearsTo(this DateTime left, DateTime right)
        {
            var years = right.Year - left.Year;
            if (left > right.AddYears(-years)) years--;

            return years;
        }
    }
}