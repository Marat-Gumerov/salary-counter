using System;
using System.Linq;
using Force.DeepCloner;
using Moq;
using NUnit.Framework;
using SalaryCounter.Service.Dao;
using SalaryCounter.Service.Model;
using SalaryCounter.Service.Service.Worker;
using SalaryCounter.Service.Service.WorkerType;
using SalaryCounter.Service.Util;
using ServiceTest.Data;

namespace ServiceTest.Test
{
    public class WorkerServiceTests
    {
        private Mock<IAppConfiguration> configurationMock;
        private Mock<IWorkerDao> workerDaoMock;
        private WorkerService workerService;
        private Mock<IWorkerTypeService> workerTypeServiceMock;

        [SetUp]
        public void SetUp()
        {
            configurationMock = new Mock<IAppConfiguration>(MockBehavior.Strict);
            configurationMock
                .Setup(configuration => configuration.Get<DateTime>(It.IsAny<string>()))
                .Returns(() => new DateTime(1899, 5, 1));

            workerTypeServiceMock = new Mock<IWorkerTypeService>(MockBehavior.Strict);
            workerTypeServiceMock
                .Setup(service => service.IsValid(It.IsAny<WorkerType>()))
                .Returns((WorkerType type) => !type.Id.Equals(Guid.Empty));

            workerDaoMock = new Mock<IWorkerDao>(MockBehavior.Strict);
            workerDaoMock
                .Setup(dao => dao.Get(It.IsAny<DateTime>()))
                .Returns((DateTime date) => WorkerTestData
                    .WorkersDictionary
                    .Values
                    .Where(worker => worker.EmploymentDate <= date)
                    .ToList());
            workerDaoMock
                .Setup(dao => dao.Get(It.IsAny<Guid>()))
                .Returns((Guid id) => WorkerTestData
                    .WorkersDictionary
                    .Values
                    .First(worker => worker.Id == id));
            workerDaoMock
                .Setup(dao => dao.HasSubordinates(It.IsAny<Worker>()))
                .Returns(() => true);
            workerDaoMock
                .Setup(dao => dao.HasWrongSubordination(It.IsAny<Worker>()))
                .Returns(
                    (Worker worker) =>
                        worker.Chief == WorkerTestData.WorkersDictionary["second"].Id);

            workerService = new WorkerService(
                configurationMock.Object,
                workerDaoMock.Object,
                workerTypeServiceMock.Object);
        }

        [Test]
        public void GetAllTest()
        {
            var workers = workerService
                .Get(new DateTime(2025, 1, 1))
                .OrderBy(worker => worker.Id)
                .ToList();
            var expected = WorkerTestData
                .WorkersDictionary
                .Values
                .OrderBy(worker => worker.Id)
                .ToList();
            Assert.That(workers.SequenceEqual(expected));
        }

        [Test]
        public void GetById()
        {
            var worker = WorkerTestData.WorkersDictionary["first"];
            var workerFromService = workerService.Get(worker.Id);
            Assert.AreEqual(worker, workerFromService);
        }

        [Test]
        public void GetWithEmptyId()
        {
            Assert.Throws<ArgumentException>(
                () => workerService.Get(Guid.Empty));
        }

        [TestCase("third")]
        [TestCase("")]
        public void GetEmptySubordinates(string workerName)
        {
            workerDaoMock
                .Setup(dao => dao.GetSubordinates(
                    It.IsAny<Worker>(),
                    It.IsAny<DateTime>()))
                .Throws(new Exception());
            var worker = workerName.Equals(string.Empty)
                ? new Worker()
                : WorkerTestData.WorkersDictionary[workerName];
            var subordinates = workerService.GetSubordinates(worker, new DateTime(2025, 1, 1));
            Assert.False(subordinates.Any());
        }

        [Test]
        public void GetSubordinates()
        {
            var subordinates = WorkerTestData
                .GetWorkersByNames("second, third, fourth")
                .OrderBy(worker => worker.Id)
                .ToList();
            workerDaoMock
                .Setup(dao => dao.GetSubordinates(
                    It.IsAny<Worker>(),
                    It.IsAny<DateTime>()))
                .Returns(() => subordinates);
            var subordinatesFromService = workerService.GetSubordinates(
                    WorkerTestData.WorkersDictionary["first"],
                    new DateTime(2025, 1, 1))
                .OrderBy(worker => worker.Id)
                .ToList();
            Assert.That(subordinates.SequenceEqual(subordinatesFromService));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void SaveWorker(bool isGuidEmpty)
        {
            var saved = false;
            workerDaoMock
                .Setup(dao => dao.Save(It.IsAny<Worker>()))
                .Callback(() => saved = true)
                .Returns((Worker savedWorker) => savedWorker);

            var worker = WorkerTestData
                .WorkersDictionary["first"]
                .DeepClone();
            if (isGuidEmpty)
                worker.Id = Guid.Empty;
            else
                worker.Name = "new name";
            workerService.Save(worker);
            Assert.IsTrue(saved);
        }

        [TestCase("Worker has wrong name", typeof(ArgumentException), "", "2019-05-01", 1000.0,
            "manager", "first", "")]
        [TestCase("Worker has wrong name", typeof(ArgumentException), "   ", "2019-05-01", 1000.0,
            "manager", "first", "")]
        [TestCase("Worker hired before company foundation date", typeof(ArgumentException),
            "newName", "1898-05-01", 1000.0, "manager", "first", "")]
        [TestCase("Worker's salary base is less than zero", typeof(ArgumentException), "newName",
            "2019-05-01", -1000.0, "manager", "first", "")]
        [TestCase("Worker position is wrong", typeof(ArgumentException), "newName", "2019-05-01",
            1000.0, "", "first", "")]
        [TestCase("Worker position is wrong", typeof(ArgumentException), "newName", "2019-05-01",
            1000.0, "invalid", "first", "")]
        [TestCase("Sequence contains no matching element", typeof(InvalidOperationException),
            "newName", "2019-05-01", 1000.0, "manager", "invalid", "")]
        [TestCase("Employee should not have subordinates", typeof(ArgumentException), "newName",
            "2019-05-01", 1000.0, "employee", "first", "")]
        [TestCase("Worker has cycle in subordination", typeof(ArgumentException), "newName",
            "2019-05-01", 1000.0, "manager", "first", "second")]
        [TestCase("Sequence contains no matching element", typeof(InvalidOperationException),
            "newName", "2019-05-01", 1000.0, "manager", "first", "invalid")]
        public void SaveWrongWorker(
            string message,
            Type exceptionType,
            string name,
            DateTime employmentDate,
            decimal salaryBase,
            string workerType,
            string newId,
            string chief)
        {
            var worker = WorkerTestData
                .WorkersDictionary["first"]
                .DeepClone();
            worker.Name = name;
            worker.EmploymentDate = employmentDate;
            worker.SalaryBase = salaryBase;
            switch (workerType)
            {
                case "":
                    worker.WorkerType = null;
                    break;
                case "invalid":
                    worker.WorkerType = new WorkerType();
                    break;
                default:
                    worker.WorkerType = WorkerTypeTestData.GetByName(workerType);
                    break;
            }

            worker.Id = GetTestGuid(newId);
            worker.Chief = GetTestGuid(chief);

            var exception = Assert.Throws(
                exceptionType,
                () => workerService.Save(worker));
            Assert.AreEqual(message, exception.Message);
        }

        [Test]
        public void DeleteWorker()
        {
            var deleted = false;
            var id = WorkerTestData.WorkersDictionary["first"].Id;
            workerDaoMock
                .Setup(dao => dao.Delete(It.Is<Guid>(idArgument => id == idArgument)))
                .Callback(() => deleted = true);

            workerService.Delete(id);
            Assert.IsTrue(deleted);
        }

        private Guid GetTestGuid(string name)
        {
            switch (name)
            {
                case "invalid":
                    return Guid.NewGuid();
                case "":
                    return Guid.Empty;
                default:
                    return WorkerTestData.WorkersDictionary[name].Id;
            }
        }
    }
}