using System;
using System.Collections.Generic;
using SalaryCounter.Service.Model;

namespace SalaryCounter.Service.Dao
{
    public interface IWorkerDao
    {
        Worker Get(Guid id);
        IList<Worker> Get(DateTime date);
        IList<Worker> GetSubordinates(Worker worker, DateTime date);
        bool HasWrongSubordination(Worker worker);
        Worker Save(Worker worker);
        bool HasSubordinates(Worker worker);
        void Delete(Guid id);
    }
}