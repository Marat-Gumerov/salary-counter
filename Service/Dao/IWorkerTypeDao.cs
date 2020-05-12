using System;
using System.Collections.Generic;
using Service.Model;

namespace Service.Dao
{
    public interface IWorkerTypeDao
    {
        IList<WorkerType> Get();
        WorkerType Get(Guid id);
    }
}