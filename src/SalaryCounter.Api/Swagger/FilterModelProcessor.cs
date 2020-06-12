using System;
using System.Collections.Generic;
using System.Linq;
using NJsonSchema.Generation;
using SalaryCounter.Api.Util;
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

        private readonly FirstTouch<Type> touched;

        private static readonly string[] Generics =
        {
            "System.Collections.Generic.List",
            "System.Collections.Generic.IList",
        };

        public FilterModelProcessor()
        {
            touched = new FirstTouch<Type>();
            touched.IsFirstTouch(typeof(string));
            touched.IsFirstTouch(typeof(int));
            touched.IsFirstTouch(typeof(bool));
            touched.IsFirstTouch(typeof(DateTime));
            touched.IsFirstTouch(typeof(decimal));
            touched.IsFirstTouch(typeof(Guid));
        }

        public void Process(SchemaProcessorContext context)
        {
            foreach (var type in WithGenerics(context.Type))
            {
                if (!touched.IsFirstTouch(type)) continue;
                if (Generics.Any(i => type.FullName?.StartsWith(i) == true))
                    continue;
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