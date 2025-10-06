using CareerSpark.DataAccessLayer.Enums;
using System.Text.Json.Serialization;

namespace CareerSpark.BusinessLayer.DTOs.Response
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int SubscriptionPlanId { get; set; }
        public string SubscriptionPlanName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public OrderStatus Status { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? VnPayTransactionId { get; set; }
        public string? VnPayOrderInfo { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? VnPayResponseCode { get; set; }
        public DateTime CreatedAt { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? PaidAt { get; set; }
        public DateTime? ExpiredAt { get; set; }
    }

    public class CreateOrderResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public OrderResponse? Order { get; set; }
        public string? PaymentUrl { get; set; }
    }
}