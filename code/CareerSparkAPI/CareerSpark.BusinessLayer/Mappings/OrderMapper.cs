using CareerSpark.BusinessLayer.DTOs.Response;
using CareerSpark.DataAccessLayer.Entities;

namespace CareerSpark.BusinessLayer.Mappings
{
    public static class OrderMapper
    {
        public static OrderResponse ToResponse(Order order)
        {
            return new OrderResponse
            {
                Id = order.Id,
                UserId = order.UserId,
                UserName = order.User?.Name ?? string.Empty,
                SubscriptionPlanId = order.SubscriptionPlanId,
                SubscriptionPlanName = order.SubscriptionPlan?.Name ?? string.Empty,
                Amount = order.Amount,
                Status = order.Status,
                VnPayTransactionId = order.VnPayTransactionId,
                VnPayOrderInfo = order.VnPayOrderInfo,
                VnPayResponseCode = order.VnPayResponseCode,
                CreatedAt = order.CreatedAt,
                PaidAt = order.PaidAt,
                ExpiredAt = order.ExpiredAt
            };
        }

        public static IEnumerable<OrderResponse> ToResponseList(IEnumerable<Order> orders)
        {
            return orders.Select(ToResponse);
        }
    }
}