using System;
using System.Linq;
using Force.DeepCloner;
using Moq;
using NUnit.Framework;
using SalaryCounter.Service.Dao;
using SalaryCounter.Service.Service.WorkerType;
using ServiceTest.Data;

namespace ServiceTest.Test
{
    public class WorkerTypeServiceTests
    {
        private Mock<IWorkerTypeDao> workerTypeDaoMock;
        private WorkerTypeService workerTypeService;

        [SetUp]
        public void SetUp()
        {
            workerTypeDaoMock = new Mock<IWorkerTypeDao>(MockBehavior.Strict);
            workerTypeDaoMock
                .Setup(dao => dao.Get(It.IsAny<Guid>()))
                .Returns((Guid id) => WorkerTypeTestData.Get(id));
            workerTypeDaoMock
                .Setup(dao => dao.Get())
                .Returns(() => WorkerTypeTestData.Get());

            workerTypeService = new WorkerTypeService(workerTypeDaoMock.Object);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void TestIsValid(bool expected)
        {
            var workerType = WorkerTypeTestData
                .GetByName("employee")
                .DeepClone();
            if (!expected) workerType.CanHaveSubordinates = true;
            var actual = workerTypeService.IsValid(workerType);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetTypes()
        {
            var actual = workerTypeService.Get()
                .OrderBy(type => type.Id)
                .ToList();
            var expected = WorkerTypeTestData
                .Get()
                .OrderBy(type => type.Id)
                .ToList();
            Assert.That(actual.SequenceEqual(expected));
        }
    }
}