using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CareerSpark.BusinessLayer.DTOs.Request
{
    public class CreateOrderRequest
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "SubscriptionPlanId must be greater than 0")]
        public int SubscriptionPlanId { get; set; }

        [Required]
        [JsonIgnore]
        public int UserId { get; set; }
    }
}