using System;
using SalaryCounter.Service.Enumeration;

namespace SalaryCounter.Service.Model
{
    public sealed class EmployeeType
    {
        public EmployeeType(Guid id, EmployeeTypeName value, bool canHaveSubordinates, SalaryRatio salaryRatio)
        {
            Id = id;
            Value = value;
            CanHaveSubordinates = canHaveSubordinates;
            SalaryRatio = salaryRatio;
        }

        public Guid Id { get; set; }
        public EmployeeTypeName Value { get; set; }
        public bool CanHaveSubordinates { get; set; }
        public SalaryRatio SalaryRatio { get; set; }
    }
}