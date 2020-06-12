using System;
using System.Collections.Generic;

namespace SalaryCounter.Service.Service.Employee
{
    public interface IEmployeeService
    {
        void Delete(Guid id);
        IList<Model.Employee> Get(DateTime date);
        Model.Employee Get(Guid id);
        IList<Model.Employee> GetSubordinates(Model.Employee employee, DateTime date);
        Model.Employee Save(Model.Employee employee);
    }
}