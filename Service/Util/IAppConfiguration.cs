namespace Service
{
    public interface IAppConfiguration
    {
        T Get<T>(string configurationItem);
    }
}
