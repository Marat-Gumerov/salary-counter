using System;
using System.Collections.Generic;
using System.Linq;

namespace Service.Service.Salary
{
    public class SalaryService : ISalaryService
    {
        private class WorkerTreeItem
        {
            public Worker Worker { get; }

            public Stack<WorkerTreeItem> Subordinates { get; }

            public decimal SubordinateSalarySum { get; set; }

            public WorkerTreeItem(Worker worker)
            {
                Worker = worker ?? throw new ArgumentException("Wrong worker");
                Subordinates = new Stack<WorkerTreeItem>();
            }

            public decimal GetSalary(DateTime date)
            {
                return Worker.SalaryBase +
                    GetExperienceBonus(date) +
                    SubordinateSalarySum * Worker.WorkerType.SalaryRatio.SubordinateBonus;
            }

            private decimal GetExperienceBonus(DateTime date)
            {
                var experience = Worker.EmploymentDate.GetYearsTo(date);
                if (experience < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(date), "Wrong employment date");
                }
                var experienceBonus = Worker.WorkerType.SalaryRatio.ExperienceBonus * experience;

                var experienceBonusPercent = Math.Min(Worker.WorkerType.SalaryRatio.ExperienceBonusMaximum, experienceBonus);

                return Worker.SalaryBase * experienceBonusPercent;
            }

        }

        private IWorkerService WorkerService { get; }

        public SalaryService(IWorkerService workerService)
        {
            WorkerService = workerService;
        }

        public decimal GetSalary(Guid workerId, DateTime date)
        {
            if (workerId.Equals(Guid.Empty))
            {
                throw new ArgumentException("Worker id is empty");
            }

            var worker = WorkerService.Get(workerId);
            var subordinates = WorkerService.GetSubordinates(worker, date);
            var workerTreeItemsDictionary = AsTreeItems(subordinates);
            var roots = workerTreeItemsDictionary
                .Values
                .Where(element => element.Worker.Chief == worker.Id)
                .ToDictionary(root => root.Worker.Id);
            var workerTreeItem = new WorkerTreeItem(worker)
            {
                SubordinateSalarySum = GetSalary(workerTreeItemsDictionary, roots, date)
            };
            return workerTreeItem.GetSalary(date);
        }

        public decimal GetSalary(DateTime date)
        {
            var workers = WorkerService.Get(date);
            var workerTreeItemsDictionary = AsTreeItems(workers);
            var roots = workerTreeItemsDictionary
                .Values
                .Where(treeItem => treeItem.Worker.Chief == null)
                .ToDictionary(root => root.Worker.Id);
            return GetSalary(workerTreeItemsDictionary, roots, date);
        }

        private static decimal GetSalary(
            IDictionary<Guid, WorkerTreeItem> workerTreeItemsDictionary,
            IDictionary<Guid, WorkerTreeItem> roots,
            DateTime date)
        {
            foreach (var treeItem in workerTreeItemsDictionary.Values)
            {
                if (roots.ContainsKey(treeItem.Worker.Id)) continue;
                if (treeItem.Worker.Chief == null) throw new InvalidOperationException("Logical error");
                if (!workerTreeItemsDictionary.ContainsKey(treeItem.Worker.Chief.Value))
                {
                    throw new InvalidOperationException($"Can't determine chief for {treeItem.Worker.Name}");
                }
                workerTreeItemsDictionary[treeItem.Worker.Chief.Value]
                    .Subordinates
                    .Push(treeItem);
            }
            decimal salary = 0;
            foreach (var root in roots.Values)
            {
                var snake = new Stack<WorkerTreeItem>();
                snake.Push(root);
                while (snake.Any())
                {
                    var current = snake.Peek();
                    if (current.Subordinates.Any())
                    {
                        snake.Push(current.Subordinates.Pop());
                        continue;
                    }
                    snake.Pop();
                    if (roots.ContainsKey(current.Worker.Id)) continue;
                    var currentSalary = current.GetSalary(date);
                    snake.Peek().SubordinateSalarySum += currentSalary + current.SubordinateSalarySum;
                }
                var rootSalary = root.GetSalary(date);
                salary += rootSalary + root.SubordinateSalarySum;
            }
            return salary;
        }

        private static Dictionary<Guid, WorkerTreeItem> AsTreeItems(IEnumerable<Worker> workers)
        {
            return workers
                .Select(worker => new WorkerTreeItem(worker))
                .ToDictionary(treeItem => treeItem.Worker.Id);
        }
    }
}
