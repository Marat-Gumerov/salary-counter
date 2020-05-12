using System;
using System.Collections.Generic;

namespace Service.Service.Worker
{
    public interface IWorkerService
    {
        void Delete(Guid id);
        IList<Model.Worker> Get(DateTime date);
        Model.Worker Get(Guid id);
        IList<Model.Worker> GetSubordinates(Model.Worker worker, DateTime date);
        Model.Worker Save(Model.Worker worker);
    }
}