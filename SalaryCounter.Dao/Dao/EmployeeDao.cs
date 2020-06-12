﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SalaryCounter.Dao.Extension;
using Force.DeepCloner;
using JetBrains.Annotations;
using SalaryCounter.Service.Dao;
using SalaryCounter.Service.Exception;
using SalaryCounter.Service.Model;

namespace SalaryCounter.Dao.Dao
{
    [UsedImplicitly]
    internal class EmployeeDao : IEmployeeDao
    {
        private readonly Dictionary<Guid, Employee> employees;
        private readonly ReaderWriterLockSlim employeesLock;

        public EmployeeDao()
        {
            employees = new Dictionary<Guid, Employee>();
            employeesLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        }

        public Employee Get(Guid id)
        {
            using (employeesLock.Read())
            {
                try
                {
                    return employees[id].DeepClone();
                }
                catch (KeyNotFoundException)
                {
                    throw new SalaryCounterNotFoundException("Employee not found");
                }
            }
        }

        public IList<Employee> Get(DateTime date)
        {
            using (employeesLock.Read())
            {
                return employees
                    .Values
                    .Where(employee => employee.EmploymentDate <= date)
                    .OrderBy(employee => employee.Name)
                    .ToList()
                    .DeepClone();
            }
        }

        public IList<Employee> GetSubordinates(Employee employee, DateTime date)
        {
            var allSubordinates = new List<Employee>();
            using (employeesLock.Read())
            {
                var currentLevel = GetNearestSubordinates(employee, date);
                allSubordinates.AddRange(currentLevel);
                while (currentLevel.Any())
                {
                    var nextLevel = new List<Employee>();
                    foreach (var subordinate in currentLevel)
                        nextLevel.AddRange(GetNearestSubordinates(subordinate, date));

                    currentLevel = nextLevel;
                    allSubordinates.AddRange(currentLevel);
                }

                return allSubordinates.DeepClone();
            }
        }

        public bool HasWrongSubordination(Employee employee)
        {
            var current = employee;
            var id = employee.Id;
            using (employeesLock.Read())
            {
                while (current.Chief != null)
                {
                    if (current.Chief.Value == id) return true;

                    current = employees[current.Chief.Value];
                }

                return false;
            }
        }

        public bool HasSubordinates(Employee employee)
        {
            using (employeesLock.Read())
            {
                return employees
                    .Values
                    .Any(element => element.Chief == employee.Id);
            }
        }

        public Employee Save(Employee employee)
        {
            var clone = employee.DeepClone();
            using (employeesLock.Write())
            {
                if (clone.Id == Guid.Empty)
                {
                    clone.Id = Guid.NewGuid();
                    employees.Add(clone.Id, clone);
                }
                else
                {
                    employees[clone.Id] = clone;
                }
            }

            return clone.DeepClone();
        }

        public void Delete(Guid id)
        {
            //Read lock is better than write lock, so call Get to check if id exists
            _ = Get(id);
            using (employeesLock.Write())
            {
                employees.Remove(id);
            }
        }

        private IList<Employee> GetNearestSubordinates(Employee employee, DateTime date)
        {
            using (employeesLock.Read())
            {
                return employees
                    .Values
                    .Where(element => element.Chief == employee.Id && element.EmploymentDate <= date)
                    .ToList();
            }
        }
    }
}