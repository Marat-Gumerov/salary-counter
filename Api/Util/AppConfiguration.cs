using System;
using Microsoft.Extensions.Configuration;
using Service;

namespace Api
{
    public class AppConfiguration : IAppConfiguration
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
