using SalaryCounter.Model.Util;
using SalaryCounter.Service.Service.Employee;
using SalaryCounter.Service.Service.EmployeeType;
using SalaryCounter.Service.Service.Example;
using SalaryCounter.Service.Service.Salary;
using SalaryCounter.Service.Util;

namespace SalaryCounter.Service.Extension
{
    public static class DependencyInjectionContainerExtensions
    {
        public static IDependencyInjectionContainer ConfigureService(
            this IDependencyInjectionContainer container) =>
            container
                .AddTransient<IEmployeeTypeService, EmployeeTypeService>()
                .AddTransient<IEmployeeService, EmployeeService>()
                .AddTransient<ISalaryService, SalaryService>()
                .AddTransient<IExampleService, ExampleService>();
    }
}
