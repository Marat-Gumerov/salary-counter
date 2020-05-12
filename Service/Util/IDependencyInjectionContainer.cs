namespace Service.Util
{
    public interface IDependencyInjectionContainer
    {
        void AddTransient<TService, TImplementation>() where TService : class
            where TImplementation : class, TService;

        void AddSingleton<TService, TImplementation>() where TService : class
            where TImplementation : class, TService;
    }
}