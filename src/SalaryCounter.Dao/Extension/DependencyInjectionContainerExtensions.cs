using SalaryCounter.Dao.Dao;
using SalaryCounter.Service.Dao;
using SalaryCounter.Service.Util;

namespace SalaryCounter.Dao.Extension
{
    public static class DependencyInjectionContainerExtensions
    {
        public static void ConfigureDao(this IDependencyInjectionContainer container)
        {
            //All data stored inside Dao instance, so use singletons
            container.AddSingleton<IEmployeeTypeDao, EmployeeTypeDao>();
            container.AddSingleton<IEmployeeDao, EmployeeDao>();
        }
    }
}