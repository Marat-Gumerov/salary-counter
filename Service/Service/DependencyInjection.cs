using System;
using Service.Service.Salary;

namespace Service
{
    public static class DependencyInjection
    {
        public static void Initialize(IDependencyInjectionContainer container)
        {
            container.AddTransient<IWorkerTypeService, WorkerTypeService>();
            container.AddTransient<IWorkerService, WorkerService>();
            container.AddTransient<ISalaryService, SalaryService>();
        }
    }
}
