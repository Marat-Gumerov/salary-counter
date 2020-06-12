using SalaryCounter.Service.Dao;
using SalaryCounter.Service.Util;

namespace SalaryCounter.Dao.Dao
{
    public static class DependencyInjection
    {
        public static void Initialize(IDependencyInjectionContainer container)
        {
            //All data stored inside Dao instance, so use singletons
            container.AddSingleton<IEmployeeTypeDao, EmployeeTypeDao>();
            container.AddSingleton<IEmployeeDao, EmployeeDao>();
        }
    }
}