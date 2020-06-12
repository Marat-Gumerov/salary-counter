using System.Collections.Generic;

namespace SalaryCounter.Service.Service.EmployeeType
{
    public interface IEmployeeTypeService
    {
        IList<Model.EmployeeType> Get();
        bool IsValid(Model.EmployeeType employeeType);
    }
}