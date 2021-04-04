using System;
using System.Linq;
using Force.DeepCloner;
using Moq;
using NUnit.Framework;
using SalaryCounter.Model.Dto;
using SalaryCounter.Model.Enumeration;
using SalaryCounter.Service.Dao;
using SalaryCounter.Service.Exception;
using SalaryCounter.Service.Service.Employee;
using SalaryCounter.Service.Service.EmployeeType;
using SalaryCounter.Service.Util;
using SalaryCounter.ServiceTest.Data;

namespace SalaryCounter.ServiceTest.Test.Service
{
    public class EmployeeServiceTests
    {
        private Mock<IAppConfiguration> configurationMock = null!;
        private Mock<IEmployeeDao> employeeDaoMock = null!;
        private EmployeeService employeeService = null!;
        private Mock<IEmployeeTypeService> employeeTypeServiceMock = null!;

        [SetUp]
        public void SetUp()
        {
            configurationMock = new Mock<IAppConfiguration>(MockBehavior.Strict);
            configurationMock
                .Setup(configuration => configuration.Get<DateTime>(It.IsAny<string>()))
                .Returns(() => new DateTime(1899, 5, 1));

            employeeTypeServiceMock = new Mock<IEmployeeTypeService>(MockBehavior.Strict);
            employeeTypeServiceMock
                .Setup(service => service.IsValid(It.IsAny<EmployeeType>()))
                .Returns((EmployeeType type) => !type.Id.Equals(Guid.Empty));

            employeeDaoMock = new Mock<IEmployeeDao>(MockBehavior.Strict);
            employeeDaoMock
                .Setup(dao => dao.Get(It.IsAny<DateTime>()))
                .Returns((DateTime date) => EmployeeTestData
                    .EmployeesDictionary
                    .Values
                    .Where(employee => employee.EmploymentDate <= date)
                    .ToList());
            employeeDaoMock
                .Setup(dao => dao.Get(It.IsAny<Guid>()))
                .Returns((Guid id) => EmployeeTestData
                    .EmployeesDictionary
                    .Values
                    .First(employee => employee.Id == id));
            employeeDaoMock
                .Setup(dao => dao.HasSubordinates(It.IsAny<Employee>()))
                .Returns(() => true);
            employeeDaoMock
                .Setup(dao => dao.HasWrongSubordination(It.IsAny<Employee>()))
                .Returns(
                    (Employee employee) =>
                        employee.Chief == EmployeeTestData.EmployeesDictionary["second"].Id);

            employeeService = new EmployeeService(
                configurationMock.Object,
                employeeDaoMock.Object,
                employeeTypeServiceMock.Object);
        }

        [Test]
        public void GetAllTest()
        {
            var employees = employeeService
                .Get(new DateTime(2025, 1, 1))
                .OrderBy(employee => employee.Id)
                .ToList();
            var expected = EmployeeTestData
                .EmployeesDictionary
                .Values
                .OrderBy(employee => employee.Id)
                .ToList();
            Assert.That(employees.SequenceEqual(expected));
        }

        [Test]
        public void GetById()
        {
            var employee = EmployeeTestData.EmployeesDictionary["first"];
            var employeeFromService = employeeService.Get(employee.Id);
            Assert.AreEqual(employee, employeeFromService);
        }

        [Test]
        public void GetWithEmptyId() =>
            Assert.Throws<SalaryCounterNotFoundException>(
                () => employeeService.Get(Guid.Empty));

        [TestCase("third")]
        [TestCase("")]
        public void GetEmptySubordinates(string employeeName)
        {
            employeeDaoMock
                .Setup(dao => dao.GetSubordinates(
                    It.IsAny<Employee>(),
                    It.IsAny<DateTime>()))
                .Throws(new Exception());
            var employee = employeeName.Equals(string.Empty)
                ? new Employee("Some name", DateTime.Today, 1000m, EmployeeTypeTestData.Workman)
                : EmployeeTestData.EmployeesDictionary[employeeName];
            var subordinates = employeeService.GetSubordinates(employee, new DateTime(2025, 1, 1));
            Assert.False(subordinates.Any());
        }

        [Test]
        public void GetSubordinates()
        {
            var subordinates = EmployeeTestData
                .GetEmployeesByNames("second, third, fourth")
                .OrderBy(employee => employee.Id)
                .ToList();
            employeeDaoMock
                .Setup(dao => dao.GetSubordinates(
                    It.IsAny<Employee>(),
                    It.IsAny<DateTime>()))
                .Returns(() => subordinates);
            var subordinatesFromService = employeeService.GetSubordinates(
                    EmployeeTestData.EmployeesDictionary["first"],
                    new DateTime(2025, 1, 1))
                .OrderBy(employee => employee.Id)
                .ToList();
            Assert.That(subordinates.SequenceEqual(subordinatesFromService));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void SaveEmployee(bool isGuidEmpty)
        {
            var saved = false;
            employeeDaoMock
                .Setup(dao => dao.Save(It.IsAny<Employee>()))
                .Callback(() => saved = true)
                .Returns((Employee savedEmployee) => savedEmployee);

            var employee = EmployeeTestData
                .EmployeesDictionary["second"]
                .DeepClone();
            if (isGuidEmpty)
                employee.Id = Guid.Empty;
            else
                employee.Name = "new name";
            employeeService.Save(employee);
            Assert.IsTrue(saved);
        }

        [TestCase("Employee has wrong name", typeof(SalaryCounterInvalidInputException), "",
            "2019-05-01", 1000.0,
            "manager", "first", "")]
        [TestCase("Employee has wrong name", typeof(SalaryCounterInvalidInputException), "   ",
            "2019-05-01", 1000.0,
            "manager", "first", "")]
        [TestCase("Employee hired before company foundation date",
            typeof(SalaryCounterInvalidInputException),
            "newName", "1898-05-01", 1000.0, "manager", "first", "")]
        [TestCase("Employee's salary base is less than zero",
            typeof(SalaryCounterInvalidInputException), "newName",
            "2019-05-01", -1000.0, "manager", "first", "")]
        [TestCase("Employee position is wrong", typeof(SalaryCounterInvalidInputException),
            "newName",
            "2019-05-01",
            1000.0, "invalid", "first", "")]
        [TestCase("Workman should not have subordinates",
            typeof(SalaryCounterInvalidInputException), "newName", "2019-05-01", 1000.0, "workman",
            "first", "second")]
        [TestCase("Employee has cycle in subordination", typeof(SalaryCounterInvalidInputException),
            "newName",
            "2019-05-01", 1000.0, "manager", "first", "second")]
        public void SaveWrongEmployee(string message, Type exceptionType, string name,
            DateTime employmentDate, decimal salaryBase, string employeeType, string newId,
            string chief)
        {
            var employee = EmployeeTestData
                .EmployeesDictionary["first"]
                .DeepClone();
            employee.Name = name;
            employee.EmploymentDate = employmentDate;
            employee.SalaryBase = salaryBase;
            employee.EmployeeType = employeeType switch
            {
                "invalid" => new EmployeeType(Guid.Empty, EmployeeTypeName.Workman, false,
                    new SalaryRatio()),
                _ => EmployeeTypeTestData.GetByName(employeeType)
            };

            employee.Id = GetTestGuid(newId);
            employee.Chief = GetTestGuid(chief);

            var exception = Assert.Throws(
                exceptionType,
                () => employeeService.Save(employee));
            Assert.AreEqual(message, exception?.Message);
        }

        [Test]
        public void DeleteEmployee()
        {
            var deleted = false;
            var id = EmployeeTestData.EmployeesDictionary["first"].Id;
            employeeDaoMock
                .Setup(dao => dao.Delete(It.Is<Guid>(idArgument => id == idArgument)))
                .Callback(() => deleted = true);

            employeeService.Delete(id);
            Assert.IsTrue(deleted);
        }

        private static Guid GetTestGuid(string name) =>
            name switch
            {
                "invalid" => Guid.NewGuid(),
                "" => Guid.Empty,
                _ => EmployeeTestData.EmployeesDictionary[name].Id
            };
    }
}
