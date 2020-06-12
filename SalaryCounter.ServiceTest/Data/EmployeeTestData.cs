using System;
using System.Collections.Generic;
using System.Linq;
using SalaryCounter.Service.Model;

namespace ServiceTest.Data
{
    public static class EmployeeTestData
    {
        private static readonly Guid First = Guid.Parse("b0511795-4cca-4a32-b006-c0a6d3927614");
        private static readonly Guid Second = Guid.Parse("fefaea84-b877-4670-a969-4f04942e46bb");
        private static readonly Guid Third = Guid.Parse("79e9d4b3-173b-40e9-a1a0-4c87bf753a4e");

        public static Dictionary<string, Employee> EmployeesDictionary { get; } =
            new Dictionary<string, Employee>
            {
                {
                    "first",
                    new Employee("first", new DateTime(2018, 11, 1), 2000m,
                        EmployeeTypeTestData.Manager, First)
                },
                {
                    "second",
                    new Employee("second", new DateTime(2020, 5, 5), 1500m, EmployeeTypeTestData.Sales,
                        Second, First)
                },
                {
                    "third",
                    new Employee("third", new DateTime(2019, 7, 1), 1000m,
                        EmployeeTypeTestData.Workman, Third, Second)
                },
                {
                    "fourth",
                    new Employee("fourth", new DateTime(1900, 11, 11), 2000m,
                        EmployeeTypeTestData.Manager, Guid.NewGuid(), First)
                },
                {
                    "fifth",
                    new Employee("fifth", new DateTime(2023, 11, 11), 1000m,
                        EmployeeTypeTestData.Workman, Guid.NewGuid())
                }
            };

        public static IList<Employee> GetEmployeesByNames(string employeeNames)
        {
            var employeesSplitted = employeeNames
                .Replace(" ", "")
                .Split(',');
            return EmployeesDictionary
                .Values
                .Where(element => employeesSplitted.Contains(element.Name))
                .ToList();
        }
    }
}