using Microsoft.Extensions.DependencyInjection;
using SalaryCounter.Service.Util;

namespace SalaryCounter.Api.Util
{
    internal class DependencyInjectionContainer : IDependencyInjectionContainer
    {
        private readonly IServiceCollection services;

        private DependencyInjectionContainer(IServiceCollection services) =>
            this.services = services;

        public static IDependencyInjectionContainer WrapServices(IServiceCollection services) =>
            new DependencyInjectionContainer(services);

        public IDependencyInjectionContainer AddSingleton<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            services.AddSingleton<TService, TImplementation>();
            return this;
        }

        public IDependencyInjectionContainer AddTransient<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            services.AddTransient<TService, TImplementation>();
            return this;
        }
    }
}
