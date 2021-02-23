using System;
using System.Collections.Generic;
using System.Linq;
using SalaryCounter.Model.Dto;
using SalaryCounter.Model.Enumeration;

namespace SalaryCounter.ServiceTest.Data
{
    public static class EmployeeTypeTestData
    {
        public static EmployeeType Workman { get; } = new EmployeeType(Guid.NewGuid(),
            EmployeeTypeName.Workman, false, new SalaryRatio
            {
                ExperienceBonus = 0.03m,
                ExperienceBonusMaximum = 0.3m,
                SubordinateBonus = 0
            });

        public static EmployeeType Manager { get; } = new EmployeeType(Guid.NewGuid(),
            EmployeeTypeName.Manager, true, new SalaryRatio
            {
                ExperienceBonus = 0.05m,
                ExperienceBonusMaximum = 0.4m,
                SubordinateBonus = 0.005m
            });

        public static EmployeeType Sales { get; } = new EmployeeType(Guid.NewGuid(),
            EmployeeTypeName.Sales, true, new SalaryRatio
            {
                ExperienceBonus = 0.01m,
                ExperienceBonusMaximum = 0.35m,
                SubordinateBonus = 0.003m
            });

        private static readonly Dictionary<string, EmployeeType> Types =
            new Dictionary<string, EmployeeType>
            {
                {"workman", Workman},
                {"manager", Manager},
                {"sales", Sales}
            };

        public static EmployeeType GetByName(string name)
        {
            return Types[name];
        }

        public static EmployeeType Get(Guid id)
        {
            return Types
                .Values
                .First(type => type.Id == id);
        }

        public static IList<EmployeeType> Get()
        {
            return Types.Values.ToList();
        }
    }
}