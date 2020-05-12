using System;
using System.Collections.Generic;
using DeepEqual.Syntax;
using Service.Dao;

namespace Service.Service.WorkerType
{
    public class WorkerTypeService : IWorkerTypeService
    {
        public WorkerTypeService(IWorkerTypeDao workerTypeDao)
        {
            WorkerTypeDao = workerTypeDao;
        }

        private IWorkerTypeDao WorkerTypeDao { get; }

        public bool IsValid(Model.WorkerType workerType)
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

        public IList<Model.WorkerType> Get()
        {
            return WorkerTypeDao.Get();
        }
    }
}