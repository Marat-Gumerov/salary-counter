using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkerTypeController : Controller
    {
        public IWorkerTypeService WorkerTypeService { get; }

        public WorkerTypeController(IWorkerTypeService workerTypeService)
        {
            WorkerTypeService = workerTypeService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(WorkerTypeService.Get());
        }
    }
}
