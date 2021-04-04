using System.Collections.Generic;
using Dto = SalaryCounter.Model.Dto;

namespace SalaryCounter.Service.Service.EmployeeType
{
    public interface IEmployeeTypeService
    {
        IList<Dto.EmployeeType> Get();

        bool IsValid(Dto.EmployeeType employeeType);
    }
}
