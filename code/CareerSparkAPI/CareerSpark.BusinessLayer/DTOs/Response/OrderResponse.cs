using CareerSpark.DataAccessLayer.Entities;
using CareerSpark.DataAccessLayer.Enums;
using System.Text.Json.Serialization;

namespace CareerSpark.BusinessLayer.DTOs.Response
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int SubscriptionPlanId { get; set; }
        public string SubscriptionPlanName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public OrderStatus Status { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PayOSTransactionId { get; set; }
        public string? PayOSOrderInfo { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PayOSResponseCode { get; set; }
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

    public class OrdersPagedResponse
    {
        public IEnumerable<OrderResponse> Items { get; set; } = Enumerable.Empty<OrderResponse>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;
    }
}