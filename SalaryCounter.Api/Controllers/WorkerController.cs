using System;
using Microsoft.AspNetCore.Mvc;
using SalaryCounter.Api.Model;
using SalaryCounter.Service.Model;
using SalaryCounter.Service.Service.Worker;

namespace SalaryCounter.Api.Controllers
{
    [Route("api/[controller]")]
    public class WorkerController : Controller
    {
        private readonly IWorkerService workerService;

        public WorkerController(IWorkerService workerService)
        {
            this.workerService = workerService;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] DateTime selectionDate)
        {
            return Ok(workerService.Get(selectionDate));
        }


        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                return Ok(workerService.Get(id));
            }
            catch (ArgumentException exception)
            {
                return NotFound(new ErrorDto(exception.Message));
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Worker value)
        {
            if (value == null) return BadRequest(new ErrorDto("Worker is null"));

            if (!value.Id.Equals(Guid.Empty))
                return BadRequest(new ErrorDto("To create worker use empty guid"));

            try
            {
                return Ok(workerService.Save(value));
            }
            catch (ArgumentException exception)
            {
                return BadRequest(new ErrorDto(exception.Message));
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] Worker value)
        {
            if (value == null) return BadRequest(new ErrorDto("Worker is null"));

            if (value.Id.Equals(Guid.Empty))
                return NotFound(new ErrorDto("Worker id should not be empty when update"));

            try
            {
                return Ok(workerService.Save(value));
            }
            catch (ArgumentException exception)
            {
                return BadRequest(new ErrorDto(exception.Message));
            }
            catch (InvalidOperationException exception)
            {
                return NotFound(new ErrorDto(exception.Message));
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                workerService.Delete(id);
                return Ok();
            }
            catch (InvalidOperationException exception)
            {
                return NotFound(new ErrorDto(exception.Message));
            }
        }
    }
}