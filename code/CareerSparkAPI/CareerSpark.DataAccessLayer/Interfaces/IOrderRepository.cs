using CareerSpark.DataAccessLayer.Entities;

namespace CareerSpark.DataAccessLayer.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order?> GetOrderByIdWithDetailsAsync(int orderId);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
        Task<Order?> GetPendingOrderByUserAndPlanAsync(int userId, int subscriptionPlanId);
        Task<bool> UpdateOrderStatusAsync(int orderId, string status, string? vnPayTransactionId = null, string? vnPayResponseCode = null);
        Task<Order?> GetOrderByVnPayTransactionAsync(string vnPayTransactionId);
    }
}