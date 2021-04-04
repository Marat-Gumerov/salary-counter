using System;

namespace SalaryCounter.Model.Util
{
    /// <summary>
    ///     Example service, provides dynamically created examples
    /// </summary>
    public interface IExampleService
    {
        /// <summary>
        ///     Provides an example for a given type
        /// </summary>
        object? GetExampleOrNull(Type type);

        /// <summary>
        ///     Provides an example for a given type
        /// </summary>
        T Get<T>() where T : class;
    }
}
