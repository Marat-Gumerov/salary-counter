using SalaryCounter.Dao.Dao;
using SalaryCounter.Service.Dao;
using SalaryCounter.Service.Util;

namespace SalaryCounter.Dao.Extensions
{
    public static class DependencyInjectionContainerExtensions
    {
        public static IDependencyInjectionContainer ConfigureDao(
            this IDependencyInjectionContainer container) =>
            //All data stored inside Dao instance, so use singletons
            container
                .AddSingleton<IEmployeeTypeDao, EmployeeTypeDao>()
                .AddSingleton<IEmployeeDao, EmployeeDao>();
    }
}
