using System;
using System.Collections.Generic;
using DeepEqual.Syntax;
using SalaryCounter.Service.Dao;

namespace SalaryCounter.Service.Service.EmployeeType
{
    public class EmployeeTypeService : IEmployeeTypeService
    {
        public EmployeeTypeService(IEmployeeTypeDao employeeTypeDao) =>
            EmployeeTypeDao = employeeTypeDao;

        private IEmployeeTypeDao EmployeeTypeDao { get; }

        public bool IsValid(Model.EmployeeType employeeType)
        {
            try
            {
                var employeeTypeFromDao = EmployeeTypeDao.Get(employeeType.Id);
                return employeeTypeFromDao.IsDeepEqual(employeeType);
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public IList<Model.EmployeeType> Get() => EmployeeTypeDao.Get();
    }
}