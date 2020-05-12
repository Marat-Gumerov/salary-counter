using Microsoft.AspNetCore.Mvc;
using Service.Service.WorkerType;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkerTypeController : Controller
    {
        private readonly IWorkerTypeService workerTypeService;

        public WorkerTypeController(IWorkerTypeService workerTypeService)
        {
            this.workerTypeService = workerTypeService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(workerTypeService.Get());
        }
    }
}