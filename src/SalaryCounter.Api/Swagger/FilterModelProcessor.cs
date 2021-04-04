using System;
using System.Collections.Generic;
using System.Linq;
using NJsonSchema.Generation;
using SalaryCounter.Api.Util;
using SalaryCounter.Model.Extension;
using SalaryCounter.Service.Exception;

namespace SalaryCounter.Api.Swagger
{
    internal class FilterModelProcessor : ISchemaProcessor
    {
        private static readonly string[] Namespaces =
        {
            "SalaryCounter.Model.Dto", "SalaryCounter.Model.Enumeration",
        };

        private readonly FirstTouch<Type> touched;

        private static readonly string[] Generics =
        {
            "System.Collections.Generic.List", "System.Collections.Generic.IList",
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
            var wrongTypeAdded = WithGenerics(context.Type)
                .Where(type => touched.IsFirstTouch(type))
                .Where(type => Generics.All(i => type.FullName?.StartsWith(i) != true))
                .FirstOrDefault(type => Namespaces.All(n => type.Namespace?.StartsWith(n) != true));
            if (wrongTypeAdded == null) return;
            throw new SalaryCounterGeneralException(
                $"{wrongTypeAdded.FullName} should not be added to Swagger", true);
        }

        private static IEnumerable<Type> WithGenerics(params Type[] types) =>
            types.SelectMany(type => type
                .AsList()
                .Concat(WithGenerics(type.GenericTypeArguments)));
    }
}
