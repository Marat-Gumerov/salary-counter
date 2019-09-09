using System;
using System.Collections;
using System.Collections.Generic;
using DeepEqual.Syntax;

namespace Service
{
    public class WorkerTypeService : IWorkerTypeService
    {
        private IWorkerTypeDao WorkerTypeDao { get; }

        public WorkerTypeService(IWorkerTypeDao workerTypeDao)
        {
            WorkerTypeDao = workerTypeDao;
        }

        public bool IsValid(WorkerType workerType)
        {
            try
            {
                var workerTypeFromDao = WorkerTypeDao.Get(workerType.Id);
                return workerTypeFromDao.IsDeepEqual(workerType);
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public IList<WorkerType> Get()
        {
            return WorkerTypeDao.Get();
        }
    }
}