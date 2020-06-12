using System;
using Moq;
using NUnit.Framework;
using SalaryCounter.Service.Exception;
using SalaryCounter.Service.Model;
using SalaryCounter.Service.Service.Salary;
using SalaryCounter.Service.Service.Employee;
using ServiceTest.Data;

namespace ServiceTest.Test
{
    public class SalaryServiceTests
    {
        private SalaryService salaryService = null!;

        private Mock<IEmployeeService> employeeMock = null!;

        [SetUp]
        public void SetUp()
        {
            employeeMock = new Mock<IEmployeeService>(MockBehavior.Strict);
            salaryService = new SalaryService(employeeMock.Object);
        }

        [Test]
        public void GetSalaryForNonSavedEmployee()
        {
            SetupEmployeeServiceGetById("first");

            Assert.Throws<SalaryCounterNotFoundException>(
                () => salaryService.GetSalary(Guid.Empty, DateTime.Now));
        }

        [TestCase("2024-04-04", 5181.7018, 0.0001, "first, second, third")]
        [TestCase("2022-04-04", 7705.0709, 0.0001, "first, second, third, fourth")]
        [TestCase("2024-04-04", 8995.7018, 0.0001, "first, second, third, fourth, fifth")]
        [TestCase("1899-04-04", 0, 0.0001, "")]
        public void GetSalaryForAll(DateTime date, double expected, double delta,
            string employeeNames)
        {
            SetupEmployeeServiceGet(employeeNames);

            var salary = salaryService.GetSalary(date);
            Assert.AreEqual(expected, (double) salary, delta);
        }

        [Test]
        public void GetSalaryWithIndefiniteChief()
        {
            SetupEmployeeServiceGet("first, third, fourth");

            Assert.Throws<SalaryCounterGeneralException>(
                () => salaryService.GetSalary(new DateTime(2019, 8, 1)));
        }

        [TestCase("2024-04-04", 2527.3418, 0.0001, "first", "second, third, fourth")]
        [TestCase("2024-04-04", 1548.36, 0.01, "second", "third")]
        [TestCase("2024-04-04", 1120.0, 0.1, "third", "")]
        [TestCase("2024-04-04", 1000.0, 0.1, "fifth", "")]
        public void GetEmployeeSalary(DateTime date, double expected, double delta, string employeeName,
            string subordinates)
        {
            SetupEmployeeServiceGet("first, second, third, fourth, fifth");
            SetupEmployeeServiceGetSubordinates(subordinates);
            SetupEmployeeServiceGetById(employeeName);

            var salary =
                salaryService.GetSalary(EmployeeTestData.EmployeesDictionary[employeeName].Id, date);
            Assert.AreEqual(expected, (double) salary, delta);
        }

        [Test]
        public void GetSalaryWithWrongEmploymentDate()
        {
            SetupEmployeeServiceGet("fifth");
            Assert.Throws<SalaryCounterInvalidInputException>(
                () => salaryService.GetSalary(new DateTime(2023, 10, 10)));
        }

        private void SetupEmployeeServiceGetSubordinates(string employeeNames)
        {
            employeeMock
                .Setup(
                    service => service.GetSubordinates(
                        It.IsAny<Employee>(), It.IsAny<DateTime>()))
                .Returns(() => EmployeeTestData.GetEmployeesByNames(employeeNames));
        }

        private void SetupEmployeeServiceGet(string employeeNames)
        {
            employeeMock
                .Setup(
                    service => service.Get(
                        It.IsAny<DateTime>()))
                .Returns(() => EmployeeTestData.GetEmployeesByNames(employeeNames));
        }

        private void SetupEmployeeServiceGetById(string employeeName)
        {
            employeeMock
                .Setup(
                    service => service.Get(
                        It.IsAny<Guid>()))
                .Returns(() => EmployeeTestData.EmployeesDictionary[employeeName]);
        }
    }
}