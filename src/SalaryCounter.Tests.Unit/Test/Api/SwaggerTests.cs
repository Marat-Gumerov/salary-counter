using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SalaryCounter.ServiceTest.Utils;

namespace SalaryCounter.ServiceTest.Test.Api
{
    public class SwaggerTests : ApiTestBase
    {
        [Test]
        public async Task GetSpecTest()
        {
            var spec = await Client.GetStringAsync("/swagger/v1.0/swagger.json");
            Assert.True(spec.Contains("Employee Name"));
            Assert.True(spec.Contains("error-type"));
        }

        protected override void SetMocks(IServiceCollection services)
        {
            //do nothing
        }
    }
}
