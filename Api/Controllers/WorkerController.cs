﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Model;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class WorkerController : Controller
    {
        public IWorkerService WorkerService { get; }

        public WorkerController(IWorkerService workerService)
        {
            WorkerService = workerService;
        }
        
        [HttpGet]
        public IActionResult Get([FromQuery] DateTime selectionDate)
        {
            return Ok(WorkerService.Get(selectionDate));
        }

        
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                return Ok(WorkerService.Get(id));
            }
            catch (ArgumentException exception)
            {
                return NotFound(new ErrorDto(exception.Message));
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]Worker value)
        {
            if (value == null)
            {
                return BadRequest(new ErrorDto("Worker is null"));
            }
            if (!value.Id.Equals(Guid.Empty))
            {
                return BadRequest(new ErrorDto("To create worker use empty guid"));
            }
            try
            {
                return Ok(WorkerService.Save(value));
            }
            catch (ArgumentException exception)
            {
                return BadRequest(new ErrorDto(exception.Message));
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody]Worker value)
        {
            if (value == null)
            {
                return BadRequest(new ErrorDto("Worker is null"));
            }
            if (value.Id.Equals(Guid.Empty))
            {
                return NotFound(new ErrorDto("Worker id should not be empty when update"));
            }
            try
            {
                return Ok(WorkerService.Save(value));
            }
            catch (ArgumentException exception)
            {
                return BadRequest(new ErrorDto(exception.Message));
            }
            catch(InvalidOperationException exception)
            {
                return NotFound(new ErrorDto(exception.Message));
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                WorkerService.Delete(id);
                return Ok();
            }
            catch (InvalidOperationException exception)
            {
                return NotFound(new ErrorDto(exception.Message));
            }
        }
    }
}
