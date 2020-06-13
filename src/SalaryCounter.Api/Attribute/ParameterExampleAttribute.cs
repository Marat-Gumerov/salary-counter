using System;
using NSwag.Annotations;
using SalaryCounter.Api.Swagger;

namespace SalaryCounter.Api.Attribute
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    internal class ParameterExampleAttribute : OpenApiOperationProcessorAttribute
    {
        public ParameterExampleAttribute(string name, string exampleName, string value)
            : base(typeof(OperationParameterProcessor), name, exampleName, value)
        {
        }
        
        public ParameterExampleAttribute(string name, string value)
            : base(typeof(OperationParameterProcessor), name, "default", value)
        {
        }
    }
}