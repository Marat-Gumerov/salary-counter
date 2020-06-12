using System;
using System.Collections.Generic;
using SalaryCounter.Service.Model;

namespace SalaryCounter.Service.Dao
{
    public interface IEmployeeTypeDao
    {
        IList<EmployeeType> Get();
        EmployeeType Get(Guid id);
    }
}