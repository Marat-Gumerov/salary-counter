using System;
using System.Collections.Generic;
using SalaryCounter.Service.Dao;
using SalaryCounter.Service.Enumeration;
using SalaryCounter.Service.Exception;
using SalaryCounter.Service.Service.EmployeeType;
using SalaryCounter.Service.Util;
using Dto = SalaryCounter.Model.Dto;

namespace SalaryCounter.Service.Service.Employee
{
    internal class EmployeeService : IEmployeeService
    {
        private IAppConfiguration Configuration { get; }

        private IEmployeeDao EmployeeDao { get; }
        private IEmployeeTypeService EmployeeTypeService { get; }

        public IList<Dto.Employee> Get(DateTime date) => EmployeeDao.Get(date);

        public EmployeeService(IAppConfiguration configuration, IEmployeeDao dao,
            IEmployeeTypeService typeService)
        {
            Configuration = configuration;
            EmployeeDao = dao;
            EmployeeTypeService = typeService;
        }

        public Dto.Employee Get(Guid id)
        {
            if (id == Guid.Empty) throw new SalaryCounterNotFoundException("Employee id is empty");
            return EmployeeDao.Get(id);
        }

        public IList<Dto.Employee> GetSubordinates(Dto.Employee employee, DateTime date)
        {
            if (!employee.Id.Equals(Guid.Empty) && employee.EmployeeType.CanHaveSubordinates)
                return EmployeeDao.GetSubordinates(employee, date);
            return new List<Dto.Employee>();
        }

        public Dto.Employee Save(Dto.Employee employee)
        {
            if (string.IsNullOrWhiteSpace(employee.Name))
                throw new SalaryCounterInvalidInputException("Employee has wrong name");
            CheckEmploymentDate(employee);
            if (employee.SalaryBase < 0)
                throw new SalaryCounterInvalidInputException(
                    "Employee's salary base is less than zero");
            if (!EmployeeTypeService.IsValid(employee.EmployeeType))
                throw new SalaryCounterInvalidInputException("Employee position is wrong");
            CheckEmployeeIdentifiers(employee);
            CheckSubordination(employee);
            return EmployeeDao.Save(employee);
        }

        public void Delete(Guid id) => EmployeeDao.Delete(id);

        private void CheckEmployeeIdentifiers(Dto.Employee employee)
        {
            if (!employee.Id.Equals(Guid.Empty)) EmployeeDao.Get(employee.Id);
            if (employee.Chief.HasValue) EmployeeDao.Get(employee.Chief.Value);
        }

        private void CheckSubordination(Dto.Employee employee)
        {
            if (!employee.EmployeeType.CanHaveSubordinates && EmployeeDao.HasSubordinates(employee))
                throw new SalaryCounterInvalidInputException(
                    "Workman should not have subordinates");
            if (EmployeeDao.HasWrongSubordination(employee))
                throw new SalaryCounterInvalidInputException("Employee has cycle in subordination");
        }

        private void CheckEmploymentDate(Dto.Employee employee)
        {
            var companyFoundationDate =
                Configuration.Get<DateTime>(
                    ServiceConfigurationItem.CompanyFoundationDate.ToString());
            if (employee.EmploymentDate < companyFoundationDate)
                throw new SalaryCounterInvalidInputException(
                    "Employee hired before company foundation date");
        }
    }
}
