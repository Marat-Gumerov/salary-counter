using SalaryCounter.Model.Exception;

namespace SalaryCounter.Model.Util
{
    internal abstract class Example<T> : IExample
    {
        protected abstract T Get(IExampleService exampleService);

        public object GetExample(IExampleService exampleService) =>
            Get(exampleService) ??
            throw new SalaryCounterModelException($"Example for type {typeof(T)} is null");
    }
}
