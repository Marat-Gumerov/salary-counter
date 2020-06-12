using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SalaryCounter.Dao.Extension;
using Force.DeepCloner;
using JetBrains.Annotations;
using SalaryCounter.Dao.Enumeration;
using SalaryCounter.Model.Dto;
using SalaryCounter.Model.Enumeration;
using SalaryCounter.Service.Dao;
using SalaryCounter.Service.Exception;
using SalaryCounter.Service.Util;

namespace SalaryCounter.Dao.Dao
{
    [UsedImplicitly]
    internal class EmployeeTypeDao : IEmployeeTypeDao
    {
        private readonly Dictionary<Guid, EmployeeType> employeeTypes;
        private readonly ReaderWriterLockSlim employeeTypesLock;

        public EmployeeTypeDao(IAppConfiguration configuration)
        {
            employeeTypes = new Dictionary<Guid, EmployeeType>();
            var id = Guid.NewGuid();
            employeeTypes.Add(id,
                new EmployeeType(id, EmployeeTypeName.Workman, false,
                    CreateSalaryRatioFor(EmployeeTypeName.Workman, configuration)));
            id = Guid.NewGuid();
            employeeTypes.Add(id,
                new EmployeeType(id, EmployeeTypeName.Manager, true,
                    CreateSalaryRatioFor(EmployeeTypeName.Manager, configuration)));
            id = Guid.NewGuid();
            employeeTypes.Add(id,
                new EmployeeType(id, EmployeeTypeName.Sales, true,
                    CreateSalaryRatioFor(EmployeeTypeName.Sales, configuration)));
            employeeTypesLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        }

        public IList<EmployeeType> Get()
        {
            using (employeeTypesLock.Read())
            {
                return employeeTypes
                    .Values
                    .OrderBy(employeeType => employeeType.Value.ToString())
                    .ToList()
                    .DeepClone();
            }
        }

        public EmployeeType Get(Guid id)
        {
            using (employeeTypesLock.Read())
            {
                try
                {
                    return employeeTypes[id].DeepClone();
                }
                catch (KeyNotFoundException)
                {
                    throw new SalaryCounterNotFoundException("No such employee position");
                }
            }
        }

        private static SalaryRatio CreateSalaryRatioFor(EmployeeTypeName employeeType,
            IAppConfiguration configuration) =>
            new SalaryRatio
            {
                ExperienceBonus =
                    configuration.Get<decimal>(
                        $"{nameof(employeeType)}:{employeeType}:{DaoConfigurationItem.ExperienceBonus}"),
                ExperienceBonusMaximum =
                    configuration.Get<decimal>(
                        $"{nameof(employeeType)}:{employeeType}:{DaoConfigurationItem.ExperienceBonusMaximum}"),
                SubordinateBonus =
                    configuration.Get<decimal>(
                        $"{nameof(employeeType)}:{employeeType}:{DaoConfigurationItem.SubordinateBonus}")
            };
    }
}