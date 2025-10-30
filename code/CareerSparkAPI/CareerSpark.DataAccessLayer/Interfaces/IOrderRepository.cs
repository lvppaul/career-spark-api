using CareerSpark.DataAccessLayer.Entities;

namespace CareerSpark.DataAccessLayer.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order?> GetOrderByIdWithDetailsAsync(int orderId);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
        Task<Order?> GetPendingOrderByUserAndPlanAsync(int userId, int subscriptionPlanId);
        Task<bool> UpdateOrderStatusAsync(int orderId, string status, string? payOSTransactionId = null, string? payOSResponseCode = null);
        Task<Order?> GetOrderByPayOSTransactionAsync(string payOSTransactionId);
    }
}