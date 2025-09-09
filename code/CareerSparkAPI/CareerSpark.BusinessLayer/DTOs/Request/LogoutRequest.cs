using System.ComponentModel.DataAnnotations;

namespace CareerSpark.BusinessLayer.DTOs.Request
{
    public class LogoutRequest
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}