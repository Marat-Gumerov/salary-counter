using System;
using Microsoft.AspNetCore.Mvc;
using SalaryCounter.Service.Service.Salary;

namespace SalaryCounter.Api.Controller
{
    public class SalaryController : BaseController
    {
        private readonly ISalaryService salaryService;

        public SalaryController(ISalaryService salaryService)
        {
            this.salaryService = salaryService;
        }

        [HttpGet]
        public decimal Get(
            [FromQuery(Name = "date")] DateTime date,
            [FromQuery(Name = "workerId")] Guid? workerId = null)
        {
            return workerId.HasValue
                ? salaryService.GetSalary(workerId.Value, date)
                : salaryService.GetSalary(date);
        }
    }
}