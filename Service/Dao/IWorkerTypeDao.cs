using System;
using System.Collections.Generic;
namespace Service
{
    public interface IWorkerTypeDao
    {
        IList<WorkerType> Get();
        WorkerType Get(Guid id);
    }
}