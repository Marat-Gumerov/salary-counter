using System;
using System.Collections.Generic;
using Service;
using System.Threading;
using Force.DeepCloner;
using System.Linq;

namespace Dao
{
    public class WorkerDao : IWorkerDao
    {
        private Dictionary<Guid, Worker> workers;
        private ReaderWriterLockSlim workersLock;

        public WorkerDao()
        {
            workers = new Dictionary<Guid, Worker>();
            workersLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        }

        public Worker Get(Guid id)
        {
            using(var lockToken = workersLock.Read())
            {
                try
                {
                    return workers[id].DeepClone();
                }
                catch(KeyNotFoundException)
                {
                    throw new InvalidOperationException("Worker not found");
                }
            }
        }

        public IList<Worker> Get(DateTime date)
        {
            using (var lockToken = workersLock.Read())
            {
                return workers
                    .Values
                    .Where(worker => worker.EmploymentDate <= date)
                    .OrderBy(worker => worker.Name)
                    .ToList()
                    .DeepClone();
            }
        }

        public IList<Worker> GetSubordinates(Worker worker, DateTime date)
        {
            var allSubordinates = new List<Worker>();
            using (var lockToken = workersLock.Read())
            {
                var currentLevel = GetNearestSubordinates(worker, date);
                allSubordinates.AddRange(currentLevel);
                while(currentLevel.Any())
                {
                    var nextLevel = new List<Worker>();
                    foreach(var subordinate in currentLevel)
                    {
                        nextLevel.AddRange(GetNearestSubordinates(subordinate, date));
                    }
                    currentLevel = nextLevel;
                    allSubordinates.AddRange(currentLevel);
                }
                return allSubordinates.DeepClone();
            }
        }

        public bool HasWrongSubordination(Worker worker)
        {
            var current = worker;
            var workerId = worker.Id;
            using (var lockToken = workersLock.Read())
            {
                while(current.Chief != null)
                {
                    if (current.Chief.Value == workerId)
                    {
                        return true;
                    }
                    current = workers[current.Chief.Value];
                }
                return false;
            }
        }

        public bool HasSubordinates(Worker worker)
        {
            using (var lockToken = workersLock.Read())
            {
                return workers
                    .Values
                    .Any(element => element.Chief == worker.Id);
            }
        }

        public Worker Save(Worker worker)
        {
            var workerClone = worker.DeepClone();
            using (var lockToken = workersLock.Write())
            {
                if (workerClone.Id == Guid.Empty)
                {
                    workerClone.Id = Guid.NewGuid();
                    workers.Add(workerClone.Id, workerClone);
                }
                else
                {
                    workers[workerClone.Id] = workerClone;
                }
            }
            return workerClone.DeepClone();
        }

        public void Delete(Guid id)
        {
            //Read lock is better than write lock, so call Get to check if id exists
            _ = Get(id);
            using (var lockToken = workersLock.Write())
            {
                workers.Remove(id);
            }
        }

        private IList<Worker> GetNearestSubordinates(Worker worker, DateTime date)
        {
            using (var lockToken = workersLock.Read())
            {
                return workers
                    .Values
                    .Where(element => element.Chief == worker.Id && element.EmploymentDate >= date)
                    .ToList();
            }
        }
    }
}
