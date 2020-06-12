using System;
using System.Collections.Generic;
using System.Linq;
using NJsonSchema.Generation;
using SalaryCounter.Service.Exception;

namespace SalaryCounter.Api.Swagger
{
    internal class FilterModelProcessor : ISchemaProcessor
    {
        private static readonly string[] Namespaces =
        {
            "SalaryCounter.Model.Dto",
            "SalaryCounter.Model.Enumeration",
        };

        private readonly HashSet<Type> allowed = new HashSet<Type>
        {
            typeof(string),
            typeof(int),
            typeof(bool),
            typeof(DateTime),
            typeof(decimal),
            typeof(Guid),
        };

        private static readonly string[] Generics =
        {
            "System.Collections.Generic.List",
            "System.Collections.Generic.IList",
        };

        public void Process(SchemaProcessorContext context)
        {
            if (allowed.Contains(context.Type)) return;
            foreach (var type in WithGenerics(context.Type))
            {
                if (allowed.Contains(type)) continue;
                if (Generics.Any(i => type.FullName?.StartsWith(i) == true))
                    continue;
                allowed.Add(type);
                if (Namespaces.All(n =>
                    type.Namespace?.StartsWith(n) != true))
                    throw new SalaryCounterGeneralException(
                        $"{type.FullName} should not be added to Swagger", true);
            }
        }

        private static IEnumerable<Type> WithGenerics(params Type[] types)
        {
            foreach (var type in types)
            {
                yield return type;
                if (type.GenericTypeArguments == null) continue;
                foreach (var typeArgument in WithGenerics(type.GenericTypeArguments))
                    yield return typeArgument;
            }
        }
    }
}