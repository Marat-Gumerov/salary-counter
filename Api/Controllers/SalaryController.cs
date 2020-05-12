using System;
using Api.Model;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryController : Controller
    {
        public SalaryController(ISalaryService salaryService)
        {
            SalaryService = salaryService;
        }

        public ISalaryService SalaryService { get; }

        [HttpGet]
        public IActionResult Get(
            [FromQuery(Name = "date")] DateTime date,
            [FromQuery(Name = "workerId")] Guid? workerId = null)
        {
            try
            {
                return Ok(workerId.HasValue
                    ? SalaryService.GetSalary(workerId.Value, date)
                    : SalaryService.GetSalary(date));
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