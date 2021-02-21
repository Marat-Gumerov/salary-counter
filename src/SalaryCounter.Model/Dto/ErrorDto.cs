using Newtonsoft.Json;
using SalaryCounter.Model.Attribute;
using SalaryCounter.Model.Util;

namespace SalaryCounter.Model.Dto
{
    /// <summary>
    ///     Error model
    /// </summary>
    [ModelExample(typeof(ErrorDtoExample))]
    public class ErrorDto
    {
        ///<inheritdoc cref="ErrorDto"/>
        public ErrorDto(string message, string? errorType = null)
        {
            Message = message;
            ErrorType = errorType ?? string.Empty;
        }

        /// <summary>
        ///     Error message
        /// </summary>
        [JsonProperty]
        public string Message { get; set; }

        /// <summary>
        ///     Error type
        /// </summary>
        [JsonProperty]
        public string ErrorType { get; set; }
    }

    internal class ErrorDtoExample : Example<ErrorDto>
    {
        public override ErrorDto Get(IExampleService exampleService) =>
            new ErrorDto("Error message", "error-type");
    }
}