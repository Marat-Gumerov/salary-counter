using Microsoft.Extensions.Configuration;
using Service.Util;

namespace Api.Util
{
    internal class AppConfiguration : IAppConfiguration
    {
        private readonly IConfiguration configuration;

        public AppConfiguration(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public T Get<T>(string configurationItem)
        {
            return configuration
                .GetSection("app")
                .GetValue<T>(configurationItem);
        }
    }
}