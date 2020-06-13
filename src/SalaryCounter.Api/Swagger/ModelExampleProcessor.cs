using System;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema.Generation;
using SalaryCounter.Api.Util;
using SalaryCounter.Model.Extension;
using SalaryCounter.Model.Util;

namespace SalaryCounter.Api.Swagger
{
    internal class ModelExampleProcessor : ISchemaProcessor
    {
        private readonly IServiceProvider serviceProvider;
        private readonly FirstTouch<Type> touched;
        private readonly JsonSerializer serializer;
        private IExampleService? exampleService;

        /// <summary>
        ///     Get service only when we really need it
        /// </summary>
        private IExampleService ExampleService =>
            exampleService ??= serviceProvider.GetService<IExampleService>();

        public ModelExampleProcessor(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            serializer = JsonSerializer.Create(new JsonSerializerSettings().Configure());
            touched = new FirstTouch<Type>();
        }

        public void Process(SchemaProcessorContext context)
        {
            var type = context.Type;
            if (!touched.IsFirstTouch(type)) return;
            var example = ExampleService.GetExampleOrNull(type);
            if (example == null) return;
            context.Schema.Example = JObject.FromObject(example, serializer);
        }
    }
}