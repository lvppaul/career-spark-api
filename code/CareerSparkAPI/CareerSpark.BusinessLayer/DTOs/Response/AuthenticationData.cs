using System.Text.Json.Serialization;

namespace CareerSpark.BusinessLayer.DTOs.Response
{
    public class AuthenticationData
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AccessToken { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? RefreshToken { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public UserResponse? User { get; set; }
    }
}
