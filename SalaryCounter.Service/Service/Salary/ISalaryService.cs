using System;

namespace SalaryCounter.Service.Service.Salary
{
    public interface ISalaryService
    {
        decimal GetSalary(Guid employeeId, DateTime date);
        decimal GetSalary(DateTime date);
    }
}