using System;
using System.Collections.Generic;
using SalaryCounter.Service.Model;

namespace SalaryCounter.Service.Dao
{
    public interface IEmployeeDao
    {
        Employee Get(Guid id);
        IList<Employee> Get(DateTime date);
        IList<Employee> GetSubordinates(Employee employee, DateTime date);
        bool HasWrongSubordination(Employee employee);
        Employee Save(Employee employee);
        bool HasSubordinates(Employee employee);
        void Delete(Guid id);
    }
}