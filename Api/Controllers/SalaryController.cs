using System;
using Api.Model;
using Microsoft.AspNetCore.Mvc;
using Service.Service.Salary;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryController : Controller
    {
        private readonly ISalaryService salaryService;

        public SalaryController(ISalaryService salaryService)
        {
            this.salaryService = salaryService;
        }

        [HttpGet]
        public IActionResult Get(
            [FromQuery(Name = "date")] DateTime date,
            [FromQuery(Name = "workerId")] Guid? workerId = null)
        {
            try
            {
                return Ok(workerId.HasValue
                    ? salaryService.GetSalary(workerId.Value, date)
                    : salaryService.GetSalary(date));
            }
            catch (ArgumentException exception)
            {
                return NotFound(new ErrorDto(exception.Message));
            }
            catch (InvalidOperationException exception)
            {
                return BadRequest(new ErrorDto(exception.Message));
            }
        }
    }
}