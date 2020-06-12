using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SalaryCounter.Service.Model;
using SalaryCounter.Service.Service.EmployeeType;

namespace SalaryCounter.Api.Controller
{
    /// <summary>
    ///     Employee positions
    /// </summary>
    [ApiVersion("1.0")]
    public class EmployeeTypeController : BaseController
    {
        private readonly IEmployeeTypeService employeeTypeService;

        ///<inheritdoc cref="EmployeeTypeController"/>
        public EmployeeTypeController(IEmployeeTypeService employeeTypeService) =>
            this.employeeTypeService = employeeTypeService;

        /// <summary>
        ///     Get list of employee positions
        /// </summary>
        /// <returns>List of positions</returns>
        [HttpGet]
        public IList<EmployeeType> Get() => employeeTypeService.Get();
    }
}