using System;
using Microsoft.AspNetCore.Mvc;
using SalaryCounter.Service.Service.Salary;

namespace SalaryCounter.Api.Controller
{
    public class SalaryController : BaseController
    {
        private readonly ISalaryService salaryService;

        public SalaryController(ISalaryService salaryService) => this.salaryService = salaryService;

        [HttpGet]
        public decimal Get(
            [FromQuery] DateTime date,
            [FromQuery] Guid? employeeId = null) =>
            employeeId.HasValue
                ? salaryService.GetSalary(employeeId.Value, date)
                : salaryService.GetSalary(date);
    }
}