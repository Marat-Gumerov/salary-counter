using System;
using System.Collections.Generic;
using System.Linq;
using SalaryCounter.Service.Model;

namespace ServiceTest.Data
{
    public static class WorkerTestData
    {
        private static readonly Guid First = Guid.Parse("b0511795-4cca-4a32-b006-c0a6d3927614");
        private static readonly Guid Second = Guid.Parse("fefaea84-b877-4670-a969-4f04942e46bb");
        private static readonly Guid Third = Guid.Parse("79e9d4b3-173b-40e9-a1a0-4c87bf753a4e");

        public static Dictionary<string, Worker> WorkersDictionary { get; } =
            new Dictionary<string, Worker>
            {
                {
                    "first",
                    new Worker
                    {
                        Chief = null,
                        EmploymentDate = new DateTime(2018, 11, 1),
                        Id = First,
                        Name = "first",
                        SalaryBase = 2000m,
                        WorkerType = WorkerTypeTestData.Manager
                    }
                },
                {
                    "second",
                    new Worker
                    {
                        Chief = First,
                        EmploymentDate = new DateTime(2020, 5, 5),
                        Id = Second,
                        Name = "second",
                        SalaryBase = 1500m,
                        WorkerType = WorkerTypeTestData.Sales
                    }
                },
                {
                    "third",
                    new Worker
                    {
                        Chief = Second,
                        EmploymentDate = new DateTime(2019, 7, 1),
                        Id = Third,
                        Name = "third",
                        SalaryBase = 1000m,
                        WorkerType = WorkerTypeTestData.Employee
                    }
                },
                {
                    "fourth",
                    new Worker
                    {
                        Chief = First,
                        EmploymentDate = new DateTime(1900, 11, 11),
                        Id = Guid.NewGuid(),
                        Name = "fourth",
                        SalaryBase = 2000m,
                        WorkerType = WorkerTypeTestData.Manager
                    }
                },
                {
                    "fifth",
                    new Worker
                    {
                        Chief = null,
                        EmploymentDate = new DateTime(2023, 11, 11),
                        Id = Guid.NewGuid(),
                        Name = "fifth",
                        SalaryBase = 1000m,
                        WorkerType = WorkerTypeTestData.Employee
                    }
                }
            };

        public static IList<Worker> GetWorkersByNames(string workerNames)
        {
            var workersSplitted = workerNames
                .Replace(" ", "")
                .Split(',');
            return WorkersDictionary
                .Values
                .Where(element => workersSplitted.Contains(element.Name))
                .ToList();
        }
    }
}