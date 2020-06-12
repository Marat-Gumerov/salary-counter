using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SalaryCounter.Service.Model;
using SalaryCounter.Service.Service.EmployeeType;

namespace SalaryCounter.Api.Controller
{
    public class EmployeeTypeController : BaseController
    {
        private readonly IEmployeeTypeService employeeTypeService;

        public EmployeeTypeController(IEmployeeTypeService employeeTypeService) =>
            this.employeeTypeService = employeeTypeService;

        [HttpGet]
        public IList<EmployeeType> Get() => employeeTypeService.Get();
    }
}