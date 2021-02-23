using System;
using System.Linq;
using Force.DeepCloner;
using Moq;
using NUnit.Framework;
using SalaryCounter.Service.Dao;
using SalaryCounter.Service.Service.EmployeeType;
using SalaryCounter.ServiceTest.Data;

namespace SalaryCounter.ServiceTest.Test.Service
{
    public class EmployeeTypeServiceTests
    {
        private Mock<IEmployeeTypeDao> employeeTypeDaoMock = null!;
        private EmployeeTypeService employeeTypeService = null!;

        [SetUp]
        public void SetUp()
        {
            employeeTypeDaoMock = new Mock<IEmployeeTypeDao>(MockBehavior.Strict);
            employeeTypeDaoMock
                .Setup(dao => dao.Get(It.IsAny<Guid>()))
                .Returns((Guid id) => EmployeeTypeTestData.Get(id));
            employeeTypeDaoMock
                .Setup(dao => dao.Get())
                .Returns(() => EmployeeTypeTestData.Get());

            employeeTypeService = new EmployeeTypeService(employeeTypeDaoMock.Object);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void TestIsValid(bool expected)
        {
            var employeeType = EmployeeTypeTestData
                .GetByName("workman")
                .DeepClone();
            if (!expected) employeeType.CanHaveSubordinates = true;
            var actual = employeeTypeService.IsValid(employeeType);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetTypes()
        {
            var actual = employeeTypeService.Get()
                .OrderBy(type => type.Id)
                .ToList();
            var expected = EmployeeTypeTestData
                .Get()
                .OrderBy(type => type.Id)
                .ToList();
            Assert.That(actual.SequenceEqual(expected));
        }
    }
}