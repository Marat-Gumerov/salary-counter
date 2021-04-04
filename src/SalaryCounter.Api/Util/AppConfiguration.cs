using Microsoft.Extensions.Configuration;
using SalaryCounter.Service.Util;

namespace SalaryCounter.Api.Util
{
    internal class AppConfiguration : IAppConfiguration
    {
        private readonly IConfiguration configuration;

        public AppConfiguration(IConfiguration configuration) => this.configuration = configuration;

        public T Get<T>(string configurationItem) =>
            configuration
                .GetSection("app")
                .GetValue<T>(configurationItem);
    }
}
