using System;

namespace Service.Service.Salary
{
    public interface ISalaryService
    {
        decimal GetSalary(Guid workerId, DateTime date);
        decimal GetSalary(DateTime date);
    }
}