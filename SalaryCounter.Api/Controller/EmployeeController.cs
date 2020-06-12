using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SalaryCounter.Service.Exception;
using SalaryCounter.Service.Model;
using SalaryCounter.Service.Service.Employee;

namespace SalaryCounter.Api.Controller
{
    public class EmployeeController : BaseController
    {
        private readonly IEmployeeService employeeService;

        public EmployeeController(IEmployeeService employeeService) =>
            this.employeeService = employeeService;

        [HttpGet]
        public IList<Employee> Get([FromQuery] DateTime selectionDate) =>
            employeeService.Get(selectionDate);


        [HttpGet("{id}")]
        public Employee Get(Guid id) => employeeService.Get(id);

        [HttpPost]
        public Employee Post([FromBody] Employee value)
        {
            if (value == null) throw new SalaryCounterGeneralException("Employee is null");

            if (!value.Id.Equals(Guid.Empty))
                throw new SalaryCounterInvalidInputException("To create employee use empty guid");

            return employeeService.Save(value);
        }

        [HttpPut]
        public Employee Put([FromBody] Employee value)
        {
            if (value == null) throw new SalaryCounterGeneralException("Employee is null");

            if (value.Id.Equals(Guid.Empty))
                throw new SalaryCounterNotFoundException(
                    "Employee id should not be empty when update");

            return employeeService.Save(value);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            employeeService.Delete(id);
            return Ok();
        }
    }
}