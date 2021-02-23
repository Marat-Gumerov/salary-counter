using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using SalaryCounter.Service.Dao;
using SalaryCounter.ServiceTest.Data;
using SalaryCounter.ServiceTest.Utils;

namespace SalaryCounter.ServiceTest.Test.Api
{
    public class SalaryTests : ApiTestBase
    {
        private Mock<IEmployeeDao> employeeDaoMock = null!;
        private Mock<IEmployeeTypeDao> employeeTypeDaoMock = null!;

        [Test]
        public async Task GetSalaryTest()
        {
            employeeDaoMock.Setup(_ => _.Get(It.IsAny<DateTime>()))
                .Returns(EmployeeTestData.GetEmployeesByNames("first, second, third"));

            var response = await Client.GetAsync("api/v1/salary?date=2024-04-04");
            var result = await response.Content.ReadAsStringAsync();
            var salary = Convert.ToDouble(result, NumberFormatInfo.InvariantInfo);
            Assert.AreEqual(5181.7018, salary, 0.0001);
        }

        protected override void SetMocks(IServiceCollection services)
        {
            employeeDaoMock = new Mock<IEmployeeDao>();
            employeeTypeDaoMock = new Mock<IEmployeeTypeDao>();
            services
                .AddSingleton(employeeDaoMock.Object)
                .AddSingleton(employeeTypeDaoMock.Object);
        }
    }
}