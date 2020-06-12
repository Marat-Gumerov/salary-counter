using System;
using System.Collections.Generic;
using System.Linq;
using SalaryCounter.Service.Exception;
using SalaryCounter.Service.Service.Employee;
using SalaryCounter.Service.Extension;

namespace SalaryCounter.Service.Service.Salary
{
    internal class SalaryService : ISalaryService
    {
        private IEmployeeService EmployeeService { get; }

        public SalaryService(IEmployeeService employeeService) => EmployeeService = employeeService;

        public decimal GetSalary(Guid employeeId, DateTime date)
        {
            if (employeeId.Equals(Guid.Empty))
                throw new SalaryCounterNotFoundException("Employee id is empty");

            var employee = EmployeeService.Get(employeeId);
            var subordinates = EmployeeService.GetSubordinates(employee, date);
            var treeItemsDictionary = AsTreeItems(subordinates);
            var roots = treeItemsDictionary
                .Values
                .Where(element => element.Employee.Chief == employee.Id)
                .ToDictionary(root => root.Employee.Id);
            var treeItem = new EmployeeTreeItem(employee)
            {
                SubordinateSalarySum = GetSalary(treeItemsDictionary, roots, date)
            };
            return treeItem.GetSalary(date);
        }

        public decimal GetSalary(DateTime date)
        {
            var employees = EmployeeService.Get(date);
            var treeItemsDictionary = AsTreeItems(employees);
            var roots = treeItemsDictionary
                .Values
                .Where(treeItem => treeItem.Employee.Chief == null)
                .ToDictionary(root => root.Employee.Id);
            return GetSalary(treeItemsDictionary, roots, date);
        }

        private static decimal GetSalary(
            IDictionary<Guid, EmployeeTreeItem> treeItemsDictionary,
            IDictionary<Guid, EmployeeTreeItem> roots,
            DateTime date)
        {
            foreach (var treeItem in treeItemsDictionary.Values)
            {
                if (roots.ContainsKey(treeItem.Employee.Id)) continue;
                if (treeItem.Employee.Chief == null)
                    throw new SalaryCounterGeneralException("Logical error", true);
                if (!treeItemsDictionary.ContainsKey(treeItem.Employee.Chief.Value))
                    throw new SalaryCounterGeneralException(
                        $"Can't determine chief for {treeItem.Employee.Name}");
                treeItemsDictionary[treeItem.Employee.Chief.Value]
                    .Subordinates
                    .Push(treeItem);
            }

            decimal salary = 0;
            foreach (var root in roots.Values)
            {
                var snake = new Stack<EmployeeTreeItem>();
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
                    if (roots.ContainsKey(current.Employee.Id)) continue;
                    var currentSalary = current.GetSalary(date);
                    snake.Peek().SubordinateSalarySum +=
                        currentSalary + current.SubordinateSalarySum;
                }

                var rootSalary = root.GetSalary(date);
                salary += rootSalary + root.SubordinateSalarySum;
            }

            return salary;
        }

        private static Dictionary<Guid, EmployeeTreeItem> AsTreeItems(
            IEnumerable<Model.Employee> employees)
        {
            return employees
                .Select(employee => new EmployeeTreeItem(employee))
                .ToDictionary(treeItem => treeItem.Employee.Id);
        }

        private class EmployeeTreeItem
        {
            public EmployeeTreeItem(Model.Employee employee)
            {
                Employee = employee ?? throw new SalaryCounterGeneralException("Wrong employee");
                Subordinates = new Stack<EmployeeTreeItem>();
            }

            public Model.Employee Employee { get; }

            public Stack<EmployeeTreeItem> Subordinates { get; }

            public decimal SubordinateSalarySum { get; set; }

            public decimal GetSalary(DateTime date)
            {
                return Employee.SalaryBase +
                       GetExperienceBonus(date) +
                       SubordinateSalarySum * Employee.EmployeeType.SalaryRatio.SubordinateBonus;
            }

            private decimal GetExperienceBonus(DateTime date)
            {
                var experience = Employee.EmploymentDate.GetYearsTo(date);
                if (experience < 0)
                    throw new SalaryCounterInvalidInputException($"Wrong employment date {date}");
                var experienceBonus = Employee.EmployeeType.SalaryRatio.ExperienceBonus * experience;

                var experienceBonusPercent =
                    Math.Min(Employee.EmployeeType.SalaryRatio.ExperienceBonusMaximum, experienceBonus);

                return Employee.SalaryBase * experienceBonusPercent;
            }
        }
    }
}