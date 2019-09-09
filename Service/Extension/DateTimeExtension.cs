﻿using System;
namespace Service
{
    public static class DateTimeExtension
    {
        public static int GetYearsTo(this DateTime left, DateTime right)
        {
            var years = right.Year - left.Year;
            if (left > right.AddYears(-years))
            {
                years--;
            }
            return years;
        }
    }
}
