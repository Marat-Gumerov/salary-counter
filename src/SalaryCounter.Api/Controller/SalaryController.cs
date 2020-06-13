using System;
using Microsoft.AspNetCore.Mvc;
using SalaryCounter.Api.Attribute;
using SalaryCounter.Service.Service.Salary;

namespace SalaryCounter.Api.Controller
{
    /// <summary>
    ///     Salary counter operations
    /// </summary>
    [ApiVersion("1.0")]
    public class SalaryController : BaseController
    {
        private readonly ISalaryService salaryService;

        ///<inheritdoc cref="SalaryController"/>
        public SalaryController(ISalaryService salaryService) => this.salaryService = salaryService;

        /// <summary>
        ///     Get salary for employee or sum for all employees
        /// </summary>
        /// <param name="date">Date to count experience bonus</param>
        /// <param name="employeeId">Employee identifier</param>
        /// <returns>Salary amount</returns>
        [DateExample("date", 0)]
        [HttpGet]
        public decimal Get(
            [FromQuery] DateTime date,
            [FromQuery] Guid? employeeId = null) =>
            employeeId.HasValue
                ? salaryService.GetSalary(employeeId.Value, date)
                : salaryService.GetSalary(date);
    }
}