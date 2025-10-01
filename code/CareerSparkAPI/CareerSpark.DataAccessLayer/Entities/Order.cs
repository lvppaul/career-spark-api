using CareerSpark.DataAccessLayer.Enums;

namespace CareerSpark.DataAccessLayer.Entities;

public partial class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int SubscriptionPlanId { get; set; }

    public decimal Amount { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public string? VnPayTransactionId { get; set; }

    public string? VnPayOrderInfo { get; set; }

    public string? VnPayResponseCode { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? PaidAt { get; set; }

    public DateTime? ExpiredAt { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual SubscriptionPlan SubscriptionPlan { get; set; } = null!;
}