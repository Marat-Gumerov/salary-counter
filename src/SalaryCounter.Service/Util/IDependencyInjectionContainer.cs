namespace SalaryCounter.Service.Util
{
    public interface IDependencyInjectionContainer
    {
        IDependencyInjectionContainer AddTransient<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;

        IDependencyInjectionContainer AddSingleton<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;
    }
}
