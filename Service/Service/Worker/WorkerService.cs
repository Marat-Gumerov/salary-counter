using System;
using System.Collections.Generic;

namespace Service
{
    public class WorkerService : IWorkerService
    {
        private IAppConfiguration Configuration { get; }
        private IWorkerDao WorkerDao { get; }
        private IWorkerTypeService WorkerTypeService { get; }

        public WorkerService(IAppConfiguration configuration, IWorkerDao workerDao, IWorkerTypeService workerTypeService)
        {
            Configuration = configuration;
            WorkerDao = workerDao;
            WorkerTypeService = workerTypeService;
        }

        public IList<Worker> Get(DateTime date)
        {
            return WorkerDao.Get(date);
        }

        public Worker Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Worker id is empty");
            }
            return WorkerDao.Get(id);
        }

        public IList<Worker> GetSubordinates(Worker worker, DateTime date)
        {
            if (worker.Id.Equals(Guid.Empty) || !worker.WorkerType.CanHaveSubordinates)
            {
                return new List<Worker>();
            }
            return WorkerDao.GetSubordinates(worker, date);
        }

        public Worker Save(Worker worker)
        {
            if (string.IsNullOrWhiteSpace(worker.Name))
            {
                throw new ArgumentException("Worker has wrong name");
            }
            var companyFoundationDate =
                Configuration.Get<DateTime>(
                    ServiceConfigurationItem.CompanyFoundationDate.ToString());
            if (worker.EmploymentDate < companyFoundationDate)
            {
                throw new ArgumentException("Worker hired before company foundation date");
            }
            if (worker.SalaryBase < 0)
            {
                throw new ArgumentException("Worker's salary base is less than zero");
            }
            if (worker.WorkerType == null || !WorkerTypeService.IsValid(worker.WorkerType))
            {
                throw new ArgumentException("Worker position is wrong");
            }
            if (!worker.Id.Equals(Guid.Empty))
            {
                WorkerDao.Get(worker.Id);
            }
            if (!worker.WorkerType.CanHaveSubordinates && WorkerDao.HasSubordinates(worker))
            {
                throw new ArgumentException("Employee should not have subordinates");
            }
            if (WorkerDao.HasWrongSubordination(worker))
            {
                throw new ArgumentException("Worker has cycle in subordination");
            }
            if (worker.Chief.HasValue)
            {
                WorkerDao.Get(worker.Chief.Value);
            }
            return WorkerDao.Save(worker);
        }

        public void Delete(Guid id)
        {
            WorkerDao.Delete(id);
        }
    }
}