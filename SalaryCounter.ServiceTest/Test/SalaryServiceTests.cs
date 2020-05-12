using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SalaryCounter.Service.Model;
using SalaryCounter.Service.Service.Salary;
using SalaryCounter.Service.Service.Worker;
using ServiceTest.Data;

namespace ServiceTest.Test
{
    public class SalaryServiceTests
    {
        private SalaryService salaryService;

        private Mock<IWorkerService> workerMock;

        [SetUp]
        public void SetUp()
        {
            workerMock = new Mock<IWorkerService>(MockBehavior.Strict);
            salaryService = new SalaryService(workerMock.Object);
        }

        [Test]
        public void GetSalaryForNonSavedWorker()
        {
            SetupWorkerServiceGetById("first");

            Assert.Throws<ArgumentException>(
                () => salaryService?.GetSalary(Guid.Empty, DateTime.Now));
        }

        [Test]
        public void GetSalaryForNullWorker()
        {
            workerMock?
                .Setup(
                    service => service.Get(
                        It.IsAny<DateTime>()))
                .Returns(() => new List<Worker> {WorkerTestData.WorkersDictionary["first"], null});
            Assert.Throws<ArgumentException>(
                () => salaryService?.GetSalary(new DateTime(2025, 1, 1)));
        }

        [TestCase("2024-04-04", 5181.7018, 0.0001, "first, second, third")]
        [TestCase("2022-04-04", 7705.0709, 0.0001, "first, second, third, fourth")]
        [TestCase("2024-04-04", 8995.7018, 0.0001, "first, second, third, fourth, fifth")]
        [TestCase("1899-04-04", 0, 0.0001, "")]
        public void GetSalaryForAll(DateTime date, double expected, double delta,
            string workerNames)
        {
            SetupWorkerServiceGet(workerNames);

            var salary = salaryService.GetSalary(date);
            Assert.AreEqual(expected, (double) salary, delta);
        }

        [Test]
        public void GetSalaryWithIndefiniteChief()
        {
            SetupWorkerServiceGet("first, third, fourth");

            Assert.Throws<InvalidOperationException>(
                () => salaryService.GetSalary(new DateTime(2019, 8, 1)));
        }

        [TestCase("2024-04-04", 2527.3418, 0.0001, "first", "second, third, fourth")]
        [TestCase("2024-04-04", 1548.36, 0.01, "second", "third")]
        [TestCase("2024-04-04", 1120.0, 0.1, "third", "")]
        [TestCase("2024-04-04", 1000.0, 0.1, "fifth", "")]
        public void GetWorkerSalary(DateTime date, double expected, double delta, string workerName,
            string subordinates)
        {
            SetupWorkerServiceGet("first, second, third, fourth, fifth");
            SetupWorkerServiceGetSubordinates(subordinates);
            SetupWorkerServiceGetById(workerName);

            var salary =
                salaryService.GetSalary(WorkerTestData.WorkersDictionary[workerName].Id, date);
            Assert.AreEqual(expected, (double) salary, delta);
        }

        [Test]
        public void GetSalaryWithWrongEmploymentDate()
        {
            SetupWorkerServiceGet("fifth");
            Assert.Throws<ArgumentOutOfRangeException>(
                () => salaryService.GetSalary(new DateTime(2023, 10, 10)));
        }

        private void SetupWorkerServiceGetSubordinates(string workerNames)
        {
            workerMock
                .Setup(
                    service => service.GetSubordinates(
                        It.IsAny<Worker>(), It.IsAny<DateTime>()))
                .Returns(() => WorkerTestData.GetWorkersByNames(workerNames));
        }

        private void SetupWorkerServiceGet(string workerNames)
        {
            workerMock
                .Setup(
                    service => service.Get(
                        It.IsAny<DateTime>()))
                .Returns(() => WorkerTestData.GetWorkersByNames(workerNames));
        }

        private void SetupWorkerServiceGetById(string workerName)
        {
            workerMock
                .Setup(
                    service => service.Get(
                        It.IsAny<Guid>()))
                .Returns(() => WorkerTestData.WorkersDictionary[workerName]);
        }
    }
}