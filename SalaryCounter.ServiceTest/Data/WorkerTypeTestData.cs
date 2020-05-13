using System;
using System.Collections.Generic;
using System.Linq;
using SalaryCounter.Service.Enumeration;
using SalaryCounter.Service.Model;

namespace ServiceTest.Data
{
    public static class WorkerTypeTestData
    {
        public static WorkerType Employee { get; } = new WorkerType(Guid.NewGuid(),
            WorkerTypeName.Employee, false, new SalaryRatio
            {
                ExperienceBonus = 0.03m,
                ExperienceBonusMaximum = 0.3m,
                SubordinateBonus = 0
            });

        public static WorkerType Manager { get; } = new WorkerType(Guid.NewGuid(),
            WorkerTypeName.Manager, true, new SalaryRatio
            {
                ExperienceBonus = 0.05m,
                ExperienceBonusMaximum = 0.4m,
                SubordinateBonus = 0.005m
            });

        public static WorkerType Sales { get; } = new WorkerType(Guid.NewGuid(),
            WorkerTypeName.Sales, true, new SalaryRatio
            {
                ExperienceBonus = 0.01m,
                ExperienceBonusMaximum = 0.35m,
                SubordinateBonus = 0.003m
            });

        private static readonly Dictionary<string, WorkerType> Types =
            new Dictionary<string, WorkerType>
            {
                {"employee", Employee},
                {"manager", Manager},
                {"sales", Sales}
            };

        public static WorkerType GetByName(string name)
        {
            return Types[name];
        }

        public static WorkerType Get(Guid id)
        {
            return Types
                .Values
                .First(type => type.Id == id);
        }

        public static IList<WorkerType> Get()
        {
            return Types.Values.ToList();
        }
    }
}