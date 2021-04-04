using System;
using Newtonsoft.Json;
using SalaryCounter.Model.Attribute;
using SalaryCounter.Model.Enumeration;

namespace SalaryCounter.Model.Dto
{
    /// <summary>
    ///     Employee position
    /// </summary>
    [ModelExample]
    public sealed class EmployeeType
    {
        ///<inheritdoc cref="EmployeeType"/>
        public EmployeeType(Guid id, EmployeeTypeName value, bool canHaveSubordinates,
            SalaryRatio salaryRatio)
        {
            Id = id;
            Value = value;
            CanHaveSubordinates = canHaveSubordinates;
            SalaryRatio = salaryRatio;
        }

        /// <summary>
        ///     Unique position identifier
        /// </summary>
        [JsonProperty]
        public Guid Id { get; set; }

        /// <summary>
        ///     Position name
        /// </summary>
        [JsonProperty]
        public EmployeeTypeName Value { get; set; }

        /// <summary>
        ///     Determines whether employee with this position can have subordinates or not
        /// </summary>
        [JsonProperty]
        public bool CanHaveSubordinates { get; set; }

        /// <summary>
        ///     Bonuses specification
        /// </summary>
        [JsonProperty]
        public SalaryRatio SalaryRatio { get; set; }
    }
}
