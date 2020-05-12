using System;
using SalaryCounter.Service.Enumeration;

namespace SalaryCounter.Service.Model
{
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class WorkerType
    {
        public virtual Guid Id { get; set; }
        public virtual WorkerTypeName Value { get; set; }
        public virtual bool CanHaveSubordinates { get; set; }
        public virtual SalaryRatio SalaryRatio { get; set; }
    }
}