using System;

namespace Service
{
    public interface ISalaryService
    {
        decimal GetSalary(Guid workerId, DateTime date);
        decimal GetSalary(DateTime date);
    }
}