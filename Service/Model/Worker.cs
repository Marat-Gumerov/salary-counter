using System;

namespace Service.Model
{
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class Worker
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime EmploymentDate { get; set; }
        public virtual decimal SalaryBase { get; set; }
        public virtual WorkerType WorkerType { get; set; }
        public virtual Guid? Chief { get; set; }
    }
}