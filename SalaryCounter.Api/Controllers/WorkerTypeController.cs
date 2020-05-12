using Microsoft.AspNetCore.Mvc;
using SalaryCounter.Service.Service.WorkerType;

namespace SalaryCounter.Api.Controllers
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