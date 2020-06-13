using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SalaryCounter.Api.Attribute;
using SalaryCounter.Model.Dto;
using SalaryCounter.Service.Exception;
using SalaryCounter.Service.Service.Employee;

namespace SalaryCounter.Api.Controller
{
    /// <summary>
    ///     Employee operations
    /// </summary>
    [ApiVersion("1.0")]
    public class EmployeeController : BaseController
    {
        private readonly IEmployeeService employeeService;

        ///<inheritdoc cref="EmployeeController"/>
        public EmployeeController(IEmployeeService employeeService) =>
            this.employeeService = employeeService;

        /// <summary>
        ///     Get list of employees
        /// </summary>
        /// <param name="selectionDate">Date to which employees should be already hired</param>
        /// <returns>A list of employees</returns>
        [DateExample("selectionDate", -5)]
        [HttpGet]
        public IList<Employee> Get([FromQuery] DateTime selectionDate) =>
            employeeService.Get(selectionDate);


        /// <summary>
        ///     Get employee by id
        /// </summary>
        /// <param name="id">Employee's id</param>
        /// <returns>Employee</returns>
        [HttpGet("{id}")]
        public Employee Get(Guid id) => employeeService.Get(id);

        /// <summary>
        ///     Add employee
        /// </summary>
        /// <param name="value">Employee</param>
        /// <returns>Added employee</returns>
        [HttpPost]
        public Employee Post([FromBody] Employee value)
        {
            if (value == null) throw new SalaryCounterGeneralException("Employee is null");

            if (!value.Id.Equals(Guid.Empty))
                throw new SalaryCounterInvalidInputException("To create employee use empty guid");

            return employeeService.Save(value);
        }

        /// <summary>
        ///     Edit employee
        /// </summary>
        /// <param name="value">Employee to set</param>
        /// <returns>Edited employee</returns>
        [HttpPut]
        public Employee Put([FromBody] Employee value)
        {
            if (value == null) throw new SalaryCounterGeneralException("Employee is null");

            if (value.Id.Equals(Guid.Empty))
                throw new SalaryCounterNotFoundException(
                    "Employee id should not be empty when update");

            return employeeService.Save(value);
        }

        /// <summary>
        ///     Delete employee
        /// </summary>
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            employeeService.Delete(id);
        }
    }
}