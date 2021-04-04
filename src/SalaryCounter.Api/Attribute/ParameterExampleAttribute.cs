using System;
using NSwag.Annotations;
using SalaryCounter.Api.Swagger;

namespace SalaryCounter.Api.Attribute
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    internal class ParameterExampleAttribute : OpenApiOperationProcessorAttribute
    {
        protected ParameterExampleAttribute(string name, string exampleName, string value)
            : base(typeof(OperationParameterProcessor), name, exampleName, value)
        {
        }

        protected ParameterExampleAttribute(string name, string value)
            : base(typeof(OperationParameterProcessor), name, "default", value)
        {
        }
    }
}
