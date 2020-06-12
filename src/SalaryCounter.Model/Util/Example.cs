using SalaryCounter.Model.Exception;

namespace SalaryCounter.Model.Util
{
    internal abstract class Example<T> : IExample
    {
        protected abstract T Value { get; }

        public object ExampleObject
        {
            get
            {
                if (Value != null) return Value;
                throw new SalaryCounterModelException($"Example for type {typeof(T)} is null");
            }
        }

        public static implicit operator T(Example<T> value) => value.Value;
    }
}