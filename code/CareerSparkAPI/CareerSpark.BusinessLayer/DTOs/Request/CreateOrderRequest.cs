using System.ComponentModel.DataAnnotations;

namespace CareerSpark.BusinessLayer.DTOs.Request
{
    public class CreateOrderRequest
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "SubscriptionPlanId must be greater than 0")]
        public int SubscriptionPlanId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "UserId must be greater than 0")]
        public int UserId { get; set; }
    }
}