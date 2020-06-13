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
        public static void ConfigureService(this IDependencyInjectionContainer container)
        {
            container.AddTransient<IEmployeeTypeService, EmployeeTypeService>();
            container.AddTransient<IEmployeeService, EmployeeService>();
            container.AddTransient<ISalaryService, SalaryService>();
            container.AddTransient<IExampleService, ExampleService>();
        }
    }
}