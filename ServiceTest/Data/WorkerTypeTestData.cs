using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Service;

namespace ServiceTest
{
    public static class WorkerTypeTestData
    {
        public static WorkerType Employee { get; } = new WorkerType
        {
            Id = Guid.NewGuid(),
            CanHaveSubordinates = false,
            Value = WorkerTypeName.Employee,
            SalaryRatio = new SalaryRatio
            {
                ExperienceBonus = 0.03m,
                ExperienceBonusMaximum = 0.3m,
                SubordinateBonus = 0
            }
        };

        public static WorkerType Manager { get; } = new WorkerType
        {
            Id = Guid.NewGuid(),
            CanHaveSubordinates = true,
            Value = WorkerTypeName.Manager,
            SalaryRatio = new SalaryRatio
            {
                ExperienceBonus = 0.05m,
                ExperienceBonusMaximum = 0.4m,
                SubordinateBonus = 0.005m
            }
        };

        public static WorkerType Sales { get; } = new WorkerType
        {
            Id = Guid.NewGuid(),
            CanHaveSubordinates = true,
            Value = WorkerTypeName.Sales,
            SalaryRatio = new SalaryRatio
            {
                ExperienceBonus = 0.01m,
                ExperienceBonusMaximum = 0.35m,
                SubordinateBonus = 0.003m
            }
        };

        private static readonly Dictionary<string, WorkerType> types = new Dictionary<string, WorkerType>
        {
            { "employee", Employee },
            { "manager", Manager },
            { "sales", Sales },
        };

        public static WorkerType GetByName(string name)
        {
            return types[name];
        }

        public static WorkerType Get(Guid id)
        {
            return types
                .Values
                .First(type => type.Id == id);
        }

        public static IList<WorkerType> Get()
        {
            return types.Values.ToList();
        }
    }
}
