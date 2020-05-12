using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SalaryCounter.Service.Model;
using SalaryCounter.Service.Service.WorkerType;

namespace SalaryCounter.Api.Controller
{
    public class WorkerTypeController : BaseController
    {
        private readonly IWorkerTypeService workerTypeService;

        public WorkerTypeController(IWorkerTypeService workerTypeService)
        {
            this.workerTypeService = workerTypeService;
        }

        [HttpGet]
        public IList<WorkerType> Get()
        {
            return workerTypeService.Get();
        }
    }
}