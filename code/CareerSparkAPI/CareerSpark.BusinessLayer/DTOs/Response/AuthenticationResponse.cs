using System.Text.Json.Serialization;

namespace CareerSpark.BusinessLayer.DTOs.Response
{
    public class AuthenticationResponse
    {
        public bool Success { get; set; }

        public string? Message { get; set; }
        // Trong trường họp null thì không serialize field này
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public AuthenticationData? Data { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string>? Errors { get; set; }
    }
}
