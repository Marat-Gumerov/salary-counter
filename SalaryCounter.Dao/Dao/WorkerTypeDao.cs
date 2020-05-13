using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SalaryCounter.Dao.Extension;
using Force.DeepCloner;
using JetBrains.Annotations;
using SalaryCounter.Dao.Enumeration;
using SalaryCounter.Service.Dao;
using SalaryCounter.Service.Enumeration;
using SalaryCounter.Service.Exception;
using SalaryCounter.Service.Model;
using SalaryCounter.Service.Util;

namespace SalaryCounter.Dao.Dao
{
    [UsedImplicitly]
    internal class WorkerTypeDao : IWorkerTypeDao
    {
        private readonly Dictionary<Guid, WorkerType> workerTypes;
        private readonly ReaderWriterLockSlim workerTypesLock;

        public WorkerTypeDao(IAppConfiguration configuration)
        {
            workerTypes = new Dictionary<Guid, WorkerType>();
            var id = Guid.NewGuid();
            workerTypes.Add(id,
                new WorkerType(id, WorkerTypeName.Employee, false,
                    CreateSalaryRatioFor(WorkerTypeName.Employee, configuration)));
            id = Guid.NewGuid();
            workerTypes.Add(id,
                new WorkerType(id, WorkerTypeName.Manager, true,
                    CreateSalaryRatioFor(WorkerTypeName.Manager, configuration)));
            id = Guid.NewGuid();
            workerTypes.Add(id,
                new WorkerType(id, WorkerTypeName.Sales, true,
                    CreateSalaryRatioFor(WorkerTypeName.Sales, configuration)));
            workerTypesLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        }

        public IList<WorkerType> Get()
        {
            using (workerTypesLock.Read())
            {
                return workerTypes
                    .Values
                    .OrderBy(workerType => workerType.Value.ToString())
                    .ToList()
                    .DeepClone();
            }
        }

        public WorkerType Get(Guid id)
        {
            using (workerTypesLock.Read())
            {
                try
                {
                    return workerTypes[id].DeepClone();
                }
                catch (KeyNotFoundException)
                {
                    throw new SalaryCounterNotFoundException("No such worker position");
                }
            }
        }

        private static SalaryRatio CreateSalaryRatioFor(WorkerTypeName workerType,
            IAppConfiguration configuration)
        {
            return new SalaryRatio
            {
                ExperienceBonus =
                    configuration.Get<decimal>(
                        $"workerType:{workerType}:{DaoConfigurationItem.ExperienceBonus}"),
                ExperienceBonusMaximum =
                    configuration.Get<decimal>(
                        $"workerType:{workerType}:{DaoConfigurationItem.ExperienceBonusMaximum}"),
                SubordinateBonus =
                    configuration.Get<decimal>(
                        $"workerType:{workerType}:{DaoConfigurationItem.SubordinateBonus}")
            };
        }
    }
}