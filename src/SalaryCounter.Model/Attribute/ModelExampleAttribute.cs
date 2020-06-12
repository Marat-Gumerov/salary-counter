using System;
using System.Linq;
using SalaryCounter.Model.Exception;
using SalaryCounter.Model.Util;

namespace SalaryCounter.Model.Attribute
{
    /// <summary>
    ///     Model example for swagger provider
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ModelExampleAttribute : System.Attribute
    {
        /// <summary>
        ///     An example for Swagger
        /// </summary>
        public object Example { get; }
        
        /// <inheritdoc />
        public ModelExampleAttribute(Type exampleType)
        {
            if (!exampleType.GetInterfaces().ToList().Contains(typeof(IExample)))
                throw new SalaryCounterModelException("Wrong ModelExample attribute");
            var exampleProvider = Activator.CreateInstance(exampleType) as IExample;
            Example = exampleProvider?.ExampleObject ??
                      throw new SalaryCounterModelException("Example generation error");
        }

    }
}