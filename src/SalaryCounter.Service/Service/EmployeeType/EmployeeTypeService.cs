using System;
using System.Collections.Generic;
using DeepEqual.Syntax;
using SalaryCounter.Service.Dao;
using Dto = SalaryCounter.Model.Dto;

namespace SalaryCounter.Service.Service.EmployeeType
{
    internal class EmployeeTypeService : IEmployeeTypeService
    {
        public EmployeeTypeService(IEmployeeTypeDao employeeTypeDao) =>
            EmployeeTypeDao = employeeTypeDao;

        private IEmployeeTypeDao EmployeeTypeDao { get; }

        public bool IsValid(Dto.EmployeeType employeeType)
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

        public IList<Dto.EmployeeType> Get() => EmployeeTypeDao.Get();
    }
}
