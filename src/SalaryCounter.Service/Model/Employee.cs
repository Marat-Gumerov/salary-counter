using System;

namespace SalaryCounter.Service.Model
{
    public sealed class Employee
    {
        public Employee(string name, DateTime employmentDate, decimal salaryBase, EmployeeType employeeType, Guid? id = null, Guid? chief = null)
        {
            Name = name;
            EmploymentDate = employmentDate;
            SalaryBase = salaryBase;
            EmployeeType = employeeType;
            Id = id ?? Guid.Empty;
            Chief = chief;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime EmploymentDate { get; set; }
        public decimal SalaryBase { get; set; }
        public EmployeeType EmployeeType { get; set; }
        public Guid? Chief { get; set; }
    }
}