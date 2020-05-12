using Newtonsoft.Json;

namespace Api.Model
{
    public class ErrorDto
    {
        public ErrorDto(string message)
        {
            Message = message;
        }

        [JsonProperty] public string Message { get; set; }
    }
}