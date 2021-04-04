using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Force.DeepCloner;
using JetBrains.Annotations;
using SalaryCounter.Dao.Enumeration;
using SalaryCounter.Dao.Extensions;
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
            employeeTypes = new[]
                {
                    EmployeeTypeName.Workman, EmployeeTypeName.Manager,
                    EmployeeTypeName.Sales
                }
                .Select(type => new
                {
                    id = Guid.NewGuid(),
                    type
                })
                .ToDictionary(
                    withId => withId.id,
                    withId => new EmployeeType(
                        withId.id,
                        withId.type,
                        withId.type != EmployeeTypeName.Workman,
                        CreateSalaryRatioFor(withId.type, configuration)));
            employeeTypesLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        }

        public IList<EmployeeType> Get()
        {
            using var read = employeeTypesLock.Read();
            return employeeTypes
                .Values
                .OrderBy(employeeType => employeeType.Value.ToString())
                .ToList()
                .DeepClone();
        }

        public EmployeeType Get(Guid id)
        {
            using var read = employeeTypesLock.Read();
            try
            {
                return employeeTypes[id].DeepClone();
            }
            catch (KeyNotFoundException)
            {
                throw new SalaryCounterNotFoundException("No such employee position");
            }
        }

        private static SalaryRatio CreateSalaryRatioFor(EmployeeTypeName employeeType,
            IAppConfiguration configuration)
        {
            const DaoConfigurationItem experienceBonus = DaoConfigurationItem.ExperienceBonus;
            const DaoConfigurationItem experienceBonusMaximum =
                DaoConfigurationItem.ExperienceBonusMaximum;
            const DaoConfigurationItem subordinateBonus = DaoConfigurationItem.SubordinateBonus;
            const string employeeTypeName = nameof(employeeType);
            return new SalaryRatio
            {
                ExperienceBonus =
                    configuration.Get<decimal>(
                        $"{employeeTypeName}:{employeeType}:{experienceBonus}"),
                ExperienceBonusMaximum =
                    configuration.Get<decimal>(
                        $"{employeeTypeName}:{employeeType}:{experienceBonusMaximum}"),
                SubordinateBonus =
                    configuration.Get<decimal>(
                        $"{employeeTypeName}:{employeeType}:{subordinateBonus}")
            };
        }
    }
}
