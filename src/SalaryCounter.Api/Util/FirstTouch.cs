using System.Collections.Generic;

namespace SalaryCounter.Api.Util
{
    internal class FirstTouch<T>
    {
        private readonly HashSet<T> touched = new HashSet<T>();

        public bool IsFirstTouch(T t)
        {
            var isFirst = !touched.Contains(t);
            if (isFirst) touched.Add(t);
            return isFirst;
        }
    }
}