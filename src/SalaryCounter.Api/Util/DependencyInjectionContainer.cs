using Microsoft.Extensions.DependencyInjection;
using SalaryCounter.Service.Util;

namespace SalaryCounter.Api.Util
{
    public class DependencyInjectionContainer : IDependencyInjectionContainer
    {
        private readonly IServiceCollection services;

        public DependencyInjectionContainer(IServiceCollection services) => this.services = services;

        public void AddSingleton<TService, TImplementation>() where TService : class
            where TImplementation : class, TService
        {
            services.AddSingleton<TService, TImplementation>();
        }

        public void AddTransient<TService, TImplementation>() where TService : class
            where TImplementation : class, TService
        {
            services.AddTransient<TService, TImplementation>();
        }
    }
}