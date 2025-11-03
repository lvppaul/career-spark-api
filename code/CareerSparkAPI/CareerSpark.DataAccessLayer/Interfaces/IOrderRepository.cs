using CareerSpark.DataAccessLayer.Entities;
using CareerSpark.DataAccessLayer.Helper;

namespace CareerSpark.DataAccessLayer.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order?> GetOrderByIdWithDetailsAsync(int orderId);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
        Task<Order?> GetPendingOrderByUserAndPlanAsync(int userId, int subscriptionPlanId);
        Task<bool> UpdateOrderStatusAsync(int orderId, string status, string? payOSTransactionId = null, string? payOSResponseCode = null);
        Task<Order?> GetOrderByPayOSTransactionAsync(string payOSTransactionId);
        Task<PaginatedResult<Order>> GetOrdersPagedAsync(Pagination pagination, int? year, int? month, int? day);

        // Revenue aggregations (Paid orders only)
        Task<decimal> GetTotalRevenueAsync(DateTime? start, DateTime? end);
        Task<IEnumerable<RevenueGroupInt>> GetRevenueByYearAsync();
        Task<IEnumerable<RevenueGroupInt>> GetRevenueByMonthAsync(int year);
        Task<IEnumerable<RevenueGroupInt>> GetRevenueByDayAsync(int year, int month);
    }
}