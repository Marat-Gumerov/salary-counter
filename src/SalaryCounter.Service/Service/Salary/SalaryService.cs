using System;
using System.Collections.Generic;
using System.Linq;
using SalaryCounter.Model.Extension;
using SalaryCounter.Service.Exception;
using SalaryCounter.Service.Service.Employee;
using Dto = SalaryCounter.Model.Dto;

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
            var treeItem = new EmployeeTreeItem(employee)
            {
                SubordinateSalarySum = GetSalary(date, subordinates, employee.Id)
            };
            return treeItem.GetSalary(date);
        }

        public decimal GetSalary(DateTime date)
        {
            var employees = EmployeeService.Get(date);
            return GetSalary(date, employees);
        }

        private static decimal GetSalary(DateTime date, IEnumerable<Dto.Employee> employees,
            Guid? chiefId = null)
        {
            var treeItemsDictionary = AsTreeItems(employees);
            var roots = treeItemsDictionary
                .Values
                .Where(treeItem => treeItem.Employee.Chief == chiefId)
                .ToDictionary(root => root.Employee.Id);
            return GetSalary(treeItemsDictionary, roots, date);
        }

        private static decimal GetSalary(
            IDictionary<Guid, EmployeeTreeItem> treeItemsDictionary,
            IDictionary<Guid, EmployeeTreeItem> roots,
            DateTime date)
        {
            BuildTree(treeItemsDictionary, roots);

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

        private static void BuildTree(IDictionary<Guid, EmployeeTreeItem> treeItemsDictionary,
            IDictionary<Guid, EmployeeTreeItem> roots)
        {
            foreach (var treeItem in treeItemsDictionary.Values
                .Where(treeItem => !roots.ContainsKey(treeItem.Employee.Id)))
            {
                if (treeItem.Employee.Chief == null)
                    throw new SalaryCounterGeneralException("Logical error", true);
                if (!treeItemsDictionary.ContainsKey(treeItem.Employee.Chief.Value))
                    throw new SalaryCounterGeneralException(
                        $"Can't determine chief for {treeItem.Employee.Name}");
                treeItemsDictionary[treeItem.Employee.Chief.Value]
                    .Subordinates
                    .Push(treeItem);
            }
        }

        private static Dictionary<Guid, EmployeeTreeItem> AsTreeItems(
            IEnumerable<Dto.Employee> employees) =>
            employees
                .Select(employee => new EmployeeTreeItem(employee))
                .ToDictionary(treeItem => treeItem.Employee.Id);

        private class EmployeeTreeItem
        {
            public EmployeeTreeItem(Dto.Employee employee)
            {
                Employee = employee ?? throw new SalaryCounterGeneralException("Wrong employee");
                Subordinates = new Stack<EmployeeTreeItem>();
            }

            public Dto.Employee Employee { get; }

            public Stack<EmployeeTreeItem> Subordinates { get; }

            public decimal SubordinateSalarySum { get; set; }

            public decimal GetSalary(DateTime date) =>
                Employee.SalaryBase +
                GetExperienceBonus(date) +
                SubordinateSalarySum * Employee.EmployeeType.SalaryRatio.SubordinateBonus;

            private decimal GetExperienceBonus(DateTime date)
            {
                var experience = Employee.EmploymentDate.GetYearsTo(date);
                if (experience < 0)
                    throw new SalaryCounterInvalidInputException($"Wrong employment date {date}");
                var experienceBonus =
                    Employee.EmployeeType.SalaryRatio.ExperienceBonus * experience;

                var experienceBonusPercent =
                    Math.Min(Employee.EmployeeType.SalaryRatio.ExperienceBonusMaximum,
                        experienceBonus);

                return Employee.SalaryBase * experienceBonusPercent;
            }
        }
    }
}
