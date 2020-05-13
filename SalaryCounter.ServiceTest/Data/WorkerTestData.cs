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
                    new Worker("first", new DateTime(2018, 11, 1), 2000m,
                        WorkerTypeTestData.Manager, First)
                },
                {
                    "second",
                    new Worker("second", new DateTime(2020, 5, 5), 1500m, WorkerTypeTestData.Sales,
                        Second, First)
                },
                {
                    "third",
                    new Worker("third", new DateTime(2019, 7, 1), 1000m,
                        WorkerTypeTestData.Employee, Third, Second)
                },
                {
                    "fourth",
                    new Worker("fourth", new DateTime(1900, 11, 11), 2000m,
                        WorkerTypeTestData.Manager, Guid.NewGuid(), First)
                },
                {
                    "fifth",
                    new Worker("fifth", new DateTime(2023, 11, 11), 1000m,
                        WorkerTypeTestData.Employee, Guid.NewGuid())
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