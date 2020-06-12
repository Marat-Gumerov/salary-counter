using SalaryCounter.Service.Service.Salary;
using SalaryCounter.Service.Service.Employee;
using SalaryCounter.Service.Service.EmployeeType;
using SalaryCounter.Service.Util;

namespace SalaryCounter.Service.Service
{
    public static class DependencyInjection
    {
        public static void Initialize(IDependencyInjectionContainer container)
        {
            container.AddTransient<IEmployeeTypeService, EmployeeTypeService>();
            container.AddTransient<IEmployeeService, EmployeeService>();
            container.AddTransient<ISalaryService, SalaryService>();
        }
    }
}