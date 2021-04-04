using System;
using System.Collections.Generic;
using SalaryCounter.Model.Dto;

namespace SalaryCounter.Service.Dao
{
    public interface IEmployeeTypeDao
    {
        IList<EmployeeType> Get();

        EmployeeType Get(Guid id);
    }
}
