using Newtonsoft.Json;

namespace SalaryCounter.Api.Model
{
    /// <summary>
    ///     Error model
    /// </summary>
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
        [JsonProperty] public string Message { get; set; }
        /// <summary>
        ///     Error type
        /// </summary>
        [JsonProperty] public string ErrorType { get; set; }
    }
}