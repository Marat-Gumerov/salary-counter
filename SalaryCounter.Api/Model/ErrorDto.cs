using Newtonsoft.Json;

namespace SalaryCounter.Api.Model
{
    public class ErrorDto
    {
        public ErrorDto(string message, string errorType = null)
        {
            Message = message;
            ErrorType = errorType;
        }

        [JsonProperty] public string Message { get; set; }
        [JsonProperty] public string ErrorType { get; set; }
    }
}