using CareerSpark.DataAccessLayer.Context;
using CareerSpark.DataAccessLayer.Entities;
using CareerSpark.DataAccessLayer.Enums;
using CareerSpark.DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using SysEnum = System.Enum;
namespace CareerSpark.DataAccessLayer.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(CareerSparkDbContext context) : base(context)
        {
        }
        public override async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders.Include(o => o.User).Include(o => o.SubscriptionPlan).ToListAsync();
        }
        public override async Task<Order> GetByIdAsync(int id)
        {
            return await _context.Orders.Include(o => o.User).Include(o => o.SubscriptionPlan).FirstOrDefaultAsync(o => o.Id == id);
        }
        public async Task<Order?> GetOrderByIdWithDetailsAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.SubscriptionPlan)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Include(o => o.SubscriptionPlan)
                .Include(o => o.User)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<Order?> GetPendingOrderByUserAndPlanAsync(int userId, int subscriptionPlanId)
        {
            return await _context.Orders
                .Include(o => o.SubscriptionPlan)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.UserId == userId
                    && o.SubscriptionPlanId == subscriptionPlanId
                    && o.Status == OrderStatus.Pending);
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string status, string? payOSTransactionId = null, string? payOSResponseCode = null)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return false;

            // Parse string status to enum
            if (SysEnum.TryParse<OrderStatus>(status, out var orderStatus))
            {
                order.Status = orderStatus;
            }
            else
            {
                return false; // Invalid status
            }

            if (!string.IsNullOrEmpty(payOSTransactionId))
                order.PayOSTransactionId = payOSTransactionId;
            if (!string.IsNullOrEmpty(payOSResponseCode))
                order.PayOSResponseCode = payOSResponseCode;

            if (order.Status == OrderStatus.Paid)
                order.PaidAt = DateTime.UtcNow;

            _context.Orders.Update(order);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Order?> GetOrderByPayOSTransactionAsync(string payOSTransactionId)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.SubscriptionPlan)
                .FirstOrDefaultAsync(o => o.PayOSTransactionId == payOSTransactionId);
        }
    }
}