using Service.Service.Salary;
using Service.Service.Worker;
using Service.Service.WorkerType;
using Service.Util;

namespace Service.Service
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