using System;
using System.Collections.Generic;

namespace SalaryCounter.Service.Service.Employee
{
    public interface IEmployeeService
    {
        void Delete(Guid id);

        IList<Model.Dto.Employee> Get(DateTime date);

        Model.Dto.Employee Get(Guid id);

        IList<Model.Dto.Employee> GetSubordinates(Model.Dto.Employee employee, DateTime date);

        Model.Dto.Employee Save(Model.Dto.Employee employee);
    }
}
