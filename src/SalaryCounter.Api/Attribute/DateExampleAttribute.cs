using System;
using JetBrains.Annotations;
using NSwag.Annotations;
using SalaryCounter.Api.Swagger;
using SalaryCounter.Service.Exception;

namespace SalaryCounter.Api.Attribute
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    internal class DateExampleAttribute : ParameterExampleAttribute
    {
        public DateExampleAttribute([NotNull] string name, [NotNull] string exampleName,
            [NotNull] string value) : base(name, exampleName, value)
        {
            if (DateTime.TryParse(value, out _)) return;
            throw new SalaryCounterGeneralException(
                $"DateExample generation error for parameter {name}, value:'{value}'");
        }

        public DateExampleAttribute([NotNull] string name, [NotNull] string exampleName,
            int daysDiffFromToday) : base(name, exampleName,
            DateTime.Today.AddDays(daysDiffFromToday).ToString("u"))
        {
        }

        public DateExampleAttribute([NotNull] string name, int daysDiffFromToday) : base(name,
            DateTime.Today.AddDays(daysDiffFromToday).ToString("u"))
        {
        }
    }
}