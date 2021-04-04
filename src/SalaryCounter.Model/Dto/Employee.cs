using System;
using Newtonsoft.Json;
using SalaryCounter.Model.Attribute;
using SalaryCounter.Model.Util;

namespace SalaryCounter.Model.Dto
{
    /// <summary>
    ///     Employee model
    /// </summary>
    [ModelExample(typeof(EmployeeExample))]
    public sealed class Employee
    {
        ///<inheritdoc cref="Employee"/>
        public Employee(string name, DateTime employmentDate, decimal salaryBase,
            EmployeeType employeeType, Guid? id = null, Guid? chief = null)
        {
            Name = name;
            EmploymentDate = employmentDate;
            SalaryBase = salaryBase;
            EmployeeType = employeeType;
            Id = id ?? Guid.Empty;
            Chief = chief;
        }

        /// <summary>
        ///     Unique employee identifier
        /// </summary>
        [JsonProperty]
        public Guid Id { get; set; }

        /// <summary>
        ///     Employee name
        /// </summary>
        [JsonProperty]
        public string Name { get; set; }

        /// <summary>
        ///     Employment date
        /// </summary>
        [JsonProperty]
        public DateTime EmploymentDate { get; set; }

        /// <summary>
        ///     Base salary
        /// </summary>
        [JsonProperty]
        public decimal SalaryBase { get; set; }

        /// <summary>
        ///     Employee's position
        /// </summary>
        [JsonProperty]
        public EmployeeType EmployeeType { get; set; }

        /// <summary>
        ///     Employee chief's identifier
        /// </summary>
        [JsonProperty]
        public Guid? Chief { get; set; }
    }

    internal class EmployeeExample : Example<Employee>
    {
        protected override Employee Get(IExampleService exampleService) => new(
            "Employee Name", DateTime.Today.AddYears(-1),
            1000, exampleService.Get<EmployeeType>());
    }
}
