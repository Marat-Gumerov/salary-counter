using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema.Generation;
using SalaryCounter.Api.Util;
using SalaryCounter.Model.Attribute;
using SalaryCounter.Model.Extension;

namespace SalaryCounter.Api.Swagger
{
    internal class ModelExampleProcessor : ISchemaProcessor
    {
        private readonly FirstTouch<Type> touched;
        private readonly JsonSerializer serializer;

        public ModelExampleProcessor()
        {
            serializer = JsonSerializer.Create(new JsonSerializerSettings().Configure());
            touched = new FirstTouch<Type>();
        }

        public void Process(SchemaProcessorContext context)
        {
            var type = context.Type;
            if (!touched.IsFirstTouch(type)) return;
            var example = type
                .GetAttributeOrNull<ModelExampleAttribute>()
                ?.Example;
            if (example == null) return;
            context.Schema.Example = JObject.FromObject(example, serializer);
        }
    }
}