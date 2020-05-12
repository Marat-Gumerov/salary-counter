using System;
using System.Collections.Generic;
using SalaryCounter.Service.Model;

namespace SalaryCounter.Service.Dao
{
    public interface IWorkerTypeDao
    {
        IList<WorkerType> Get();
        WorkerType Get(Guid id);
    }
}