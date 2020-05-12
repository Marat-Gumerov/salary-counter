using SalaryCounter.Service.Service.Salary;
using SalaryCounter.Service.Service.Worker;
using SalaryCounter.Service.Service.WorkerType;
using SalaryCounter.Service.Util;

namespace SalaryCounter.Service.Service
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