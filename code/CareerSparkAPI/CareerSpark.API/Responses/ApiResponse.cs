using System.Text.Json.Serialization;

namespace CareerSpark.API.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Payload { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
