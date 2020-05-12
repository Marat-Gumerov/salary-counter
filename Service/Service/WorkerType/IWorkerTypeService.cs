using System.Collections.Generic;

namespace Service.Service.WorkerType
{
    public interface IWorkerTypeService
    {
        IList<Model.WorkerType> Get();
        bool IsValid(Model.WorkerType workerType);
    }
}