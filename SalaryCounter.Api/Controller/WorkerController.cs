using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SalaryCounter.Api.Model;
using SalaryCounter.Service.Exception;
using SalaryCounter.Service.Model;
using SalaryCounter.Service.Service.Worker;

namespace SalaryCounter.Api.Controller
{
    public class WorkerController : BaseController
    {
        private readonly IWorkerService workerService;

        public WorkerController(IWorkerService workerService)
        {
            this.workerService = workerService;
        }

        [HttpGet]
        public IList<Worker> Get([FromQuery] DateTime selectionDate)
        {
            return workerService.Get(selectionDate);
        }


        [HttpGet("{id}")]
        public Worker Get(Guid id)
        {
            return workerService.Get(id);
        }

        [HttpPost]
        public Worker Post([FromBody] Worker value)
        {
            if (value == null) throw new SalaryCounterGeneralException("Worker is null");

            if (!value.Id.Equals(Guid.Empty))
                throw new SalaryCounterInvalidInputException("To create worker use empty guid");

            return workerService.Save(value);
        }

        [HttpPut]
        public Worker Put([FromBody] Worker value)
        {
            if (value == null) throw new SalaryCounterGeneralException("Worker is null");

            if (value.Id.Equals(Guid.Empty))
                throw new SalaryCounterNotFoundException(
                    "Worker id should not be empty when update");

            return workerService.Save(value);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            workerService.Delete(id);
            return Ok();
        }
    }
}