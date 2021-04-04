namespace SalaryCounter.Model.Util
{
    /// <summary>
    ///     Swagger example provider
    /// </summary>
    public interface IExample
    {
        /// <summary>
        ///     Example object
        /// </summary>
        object GetExample(IExampleService exampleService);
    }
}
