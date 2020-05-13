using System;
using SalaryCounter.Service.Enumeration;

namespace SalaryCounter.Service.Model
{
    public sealed class WorkerType
    {
        public WorkerType(Guid id, WorkerTypeName value, bool canHaveSubordinates, SalaryRatio salaryRatio)
        {
            Id = id;
            Value = value;
            CanHaveSubordinates = canHaveSubordinates;
            SalaryRatio = salaryRatio;
        }

        public Guid Id { get; set; }
        public WorkerTypeName Value { get; set; }
        public bool CanHaveSubordinates { get; set; }
        public SalaryRatio SalaryRatio { get; set; }
    }
}