using System;
using System.Collections.Generic;
using SalaryCounter.Service.Dao;
using SalaryCounter.Service.Enumeration;
using SalaryCounter.Service.Service.WorkerType;
using SalaryCounter.Service.Util;

namespace SalaryCounter.Service.Service.Worker
{
    public class WorkerService : IWorkerService
    {
        public WorkerService(IAppConfiguration configuration, IWorkerDao workerDao,
            IWorkerTypeService workerTypeService)
        {
            Configuration = configuration;
            WorkerDao = workerDao;
            WorkerTypeService = workerTypeService;
        }

        private IAppConfiguration Configuration { get; }
        private IWorkerDao WorkerDao { get; }
        private IWorkerTypeService WorkerTypeService { get; }

        public IList<Model.Worker> Get(DateTime date)
        {
            return WorkerDao.Get(date);
        }

        public Model.Worker Get(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentException("Worker id is empty");
            return WorkerDao.Get(id);
        }

        public IList<Model.Worker> GetSubordinates(Model.Worker worker, DateTime date)
        {
            if (worker.Id.Equals(Guid.Empty) || !worker.WorkerType.CanHaveSubordinates)
                return new List<Model.Worker>();
            return WorkerDao.GetSubordinates(worker, date);
        }

        public Model.Worker Save(Model.Worker worker)
        {
            if (string.IsNullOrWhiteSpace(worker.Name))
                throw new ArgumentException("Worker has wrong name");
            var companyFoundationDate =
                Configuration.Get<DateTime>(
                    ServiceConfigurationItem.CompanyFoundationDate.ToString());
            if (worker.EmploymentDate < companyFoundationDate)
                throw new ArgumentException("Worker hired before company foundation date");
            if (worker.SalaryBase < 0)
                throw new ArgumentException("Worker's salary base is less than zero");
            if (worker.WorkerType == null || !WorkerTypeService.IsValid(worker.WorkerType))
                throw new ArgumentException("Worker position is wrong");
            if (!worker.Id.Equals(Guid.Empty)) WorkerDao.Get(worker.Id);
            if (!worker.WorkerType.CanHaveSubordinates && WorkerDao.HasSubordinates(worker))
                throw new ArgumentException("Employee should not have subordinates");
            if (WorkerDao.HasWrongSubordination(worker))
                throw new ArgumentException("Worker has cycle in subordination");
            if (worker.Chief.HasValue) WorkerDao.Get(worker.Chief.Value);
            return WorkerDao.Save(worker);
        }

        public void Delete(Guid id)
        {
            WorkerDao.Delete(id);
        }
    }
}