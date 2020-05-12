using System.Collections.Generic;

namespace SalaryCounter.Service.Service.WorkerType
{
    public interface IWorkerTypeService
    {
        IList<Model.WorkerType> Get();
        bool IsValid(Model.WorkerType workerType);
    }
}