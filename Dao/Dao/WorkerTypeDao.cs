using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Force.DeepCloner;
using Service;

namespace Dao
{
    public class WorkerTypeDao : IWorkerTypeDao
    {
        private readonly Dictionary<Guid, WorkerType> workerTypes;
        private readonly ReaderWriterLockSlim workerTypesLock;

        public WorkerTypeDao(IAppConfiguration configuration)
        {
            workerTypes = new Dictionary<Guid, WorkerType>();
            var id = Guid.NewGuid();
            workerTypes.Add(id,
                new WorkerType
                { 
                    Value = WorkerTypeName.Employee,
                    CanHaveSubordinates = false,
                    Id = id,
                    SalaryRatio = CreateSalaryRatioFor(WorkerTypeName.Employee, configuration)
                });
            id = Guid.NewGuid();
            workerTypes.Add(id,
                new WorkerType
                {
                    Value = WorkerTypeName.Manager,
                    CanHaveSubordinates = true,
                    Id = id,
                    SalaryRatio = CreateSalaryRatioFor(WorkerTypeName.Manager, configuration)
                });
            id = Guid.NewGuid();
            workerTypes.Add(id,
                new WorkerType
                {
                    Value = WorkerTypeName.Sales,
                    CanHaveSubordinates = true,
                    Id = id,
                    SalaryRatio = CreateSalaryRatioFor(WorkerTypeName.Sales, configuration)
                });
            workerTypesLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        }

        public IList<WorkerType> Get()
        {
            using(var lockToken = workerTypesLock.Read())
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
            using (var lockToken = workerTypesLock.Read())
            {
                try
                {
                    return workerTypes[id].DeepClone();
                }
                catch(KeyNotFoundException)
                {
                    throw new InvalidOperationException("No such worker position"); 
                }
            }
        }

        private static SalaryRatio CreateSalaryRatioFor(WorkerTypeName workerType, IAppConfiguration configuration)
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
                                $"workerType:{workerType}:{DaoConfigurationItem.SubordinateBonus}"),
            };
        }
    }
}
