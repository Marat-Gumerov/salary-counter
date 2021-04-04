using System.Collections.Generic;
using System.Linq;
using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using SalaryCounter.Service.Exception;

namespace SalaryCounter.Api.Swagger
{
    internal class OperationParameterProcessor : IOperationProcessor
    {
        private readonly string value;
        private readonly string name;
        private readonly string exampleName;

        public OperationParameterProcessor(string name, string exampleName, string value)
        {
            this.name = name;
            this.exampleName = exampleName;
            this.value = value;
        }

        public bool Process(OperationProcessorContext context)
        {
            var operation = context.OperationDescription.Operation;
            var parameter = operation.Parameters.FirstOrDefault(item => item.Name == name);
            if (parameter == null)
                throw new SalaryCounterGeneralException(
                    $"Parameter {name} not found in {operation.OperationId}", true);
            var examples = parameter.Examples ??= new Dictionary<string, OpenApiExample>();
            examples.Add(exampleName, new OpenApiExample
            {
                Value = value
            });
            return true;
        }
    }
}
