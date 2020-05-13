using System;

namespace SalaryCounter.Service.Model
{
    public sealed class Worker
    {
        public Worker(string name, DateTime employmentDate, decimal salaryBase, WorkerType workerType, Guid? id = null, Guid? chief = null)
        {
            Name = name;
            EmploymentDate = employmentDate;
            SalaryBase = salaryBase;
            WorkerType = workerType;
            Id = id ?? Guid.Empty;
            Chief = chief;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime EmploymentDate { get; set; }
        public decimal SalaryBase { get; set; }
        public WorkerType WorkerType { get; set; }
        public Guid? Chief { get; set; }
    }
}