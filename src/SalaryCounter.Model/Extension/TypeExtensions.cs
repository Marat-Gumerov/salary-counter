using System;
using System.Linq;

namespace SalaryCounter.Model.Extension
{
    /// <summary>
    ///     Type extensions
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        ///     Get first attribute of specified type
        /// </summary>
        public static T? GetAttributeOrNull<T>(this Type type) where T : System.Attribute =>
            type.GetCustomAttributes(true).FirstOrDefault(a => a is T) as T;
    }
}