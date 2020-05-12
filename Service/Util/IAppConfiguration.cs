namespace Service.Util
{
    public interface IAppConfiguration
    {
        T Get<T>(string configurationItem);
    }
}