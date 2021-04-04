using System;

namespace SalaryCounter.Model.Attribute
{
    /// <summary>
    ///     Model example for swagger provider
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ModelExampleAttribute : System.Attribute
    {
        /// <summary>
        ///     IExample provider
        /// </summary>
        public Type? Type { get; }

        /// <inheritdoc />
        internal ModelExampleAttribute()
        {
        }

        /// <inheritdoc />
        internal ModelExampleAttribute(Type exampleType) : this() => Type = exampleType;
    }
}
