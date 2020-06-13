using System;
using System.Linq;
using SalaryCounter.Model.Attribute;
using SalaryCounter.Model.Extension;
using SalaryCounter.Model.Util;
using SalaryCounter.Service.Exception;
using SalaryCounter.Service.Service.EmployeeType;

namespace SalaryCounter.Service.Service.Example
{
    internal class ExampleService : IExampleService
    {
        private readonly IEmployeeTypeService employeeTypeService;

        public ExampleService(IEmployeeTypeService employeeTypeService) =>
            this.employeeTypeService = employeeTypeService;

        public object? GetExampleOrNull(Type type)
        {
            var attribute = type.GetAttributeOrNull<ModelExampleAttribute>();
            if (attribute == null) return null;
            return attribute.Type == null
                ? GetRealInstanceOrNull(type)
                : GetFromExampleProvider(attribute.Type);
        }

        public T Get<T>() where T : class =>
            GetExampleOrNull(typeof(T)) as T ??
            throw new SalaryCounterGeneralException("Example generation error");

        private object? GetRealInstanceOrNull(Type type)
        {
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (type == typeof(Model.Dto.EmployeeType))
                return employeeTypeService.Get().First();
            return null;
        }

        private object GetFromExampleProvider(Type type)
        {
            if (!type.GetInterfaces().ToList().Contains(typeof(IExample)))
                throw new SalaryCounterGeneralException("Wrong ModelExample attribute");
            var exampleProvider = Activator.CreateInstance(type) as IExample;
            return exampleProvider?.GetExample(this) ??
                   throw new SalaryCounterGeneralException("Example generation error");
        }
    }
}