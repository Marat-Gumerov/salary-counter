using System;
using System.Collections.Generic;
namespace Service
{
    public interface IWorkerService
    {
        void Delete(Guid id);
        IList<Worker> Get(DateTime date);
        Worker Get(Guid id);
        IList<Worker> GetSubordinates(Worker worker, DateTime date);
        Worker Save(Worker worker);
    }
}