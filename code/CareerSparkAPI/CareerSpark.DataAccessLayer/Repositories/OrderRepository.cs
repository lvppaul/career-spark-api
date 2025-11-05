using CareerSpark.DataAccessLayer.Context;
using CareerSpark.DataAccessLayer.Entities;
using CareerSpark.DataAccessLayer.Enums;
using CareerSpark.DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using SysEnum = System.Enum;
using CareerSpark.DataAccessLayer.Helper;

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

        public async Task<PaginatedResult<Order>> GetOrdersPagedAsync(Pagination pagination, int? year, int? month, int? day)
        {
            var query = _context.Orders
                .Include(o => o.User)
                .Include(o => o.SubscriptionPlan)
                .AsQueryable();

            // Build date range based on provided year/month/day for CreatedAt
            DateTime? start = null;
            DateTime? end = null;

            if (year.HasValue)
            {
                if (month.HasValue)
                {
                    if (day.HasValue)
                    {
                        start = new DateTime(year.Value, month.Value, day.Value, 0, 0, 0, DateTimeKind.Utc);
                        end = start.Value.AddDays(1);
                    }
                    else
                    {
                        start = new DateTime(year.Value, month.Value, 1, 0, 0, 0, DateTimeKind.Utc);
                        end = start.Value.AddMonths(1);
                    }
                }
                else
                {
                    start = new DateTime(year.Value, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    end = start.Value.AddYears(1);
                }
            }

            if (start.HasValue && end.HasValue)
            {
                query = query.Where(o => o.CreatedAt >= start.Value && o.CreatedAt < end.Value);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(o => o.CreatedAt)
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();

            return new PaginatedResult<Order>(items, totalCount, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<decimal> GetTotalRevenueAsync(DateTime? start, DateTime? end)
        {
            var query = _context.Orders.Where(o => o.Status == OrderStatus.Paid);

            if (start.HasValue)
                query = query.Where(o => o.CreatedAt >= start.Value);
            if (end.HasValue)
                query = query.Where(o => o.CreatedAt < end.Value);

            return await query.SumAsync(o => o.Amount);
        }

        public async Task<IEnumerable<RevenueGroupInt>> GetRevenueByYearAsync()
        {
            return await _context.Orders
                .Where(o => o.Status == OrderStatus.Paid)
                .GroupBy(o => o.CreatedAt.Year)
                .Select(g => new RevenueGroupInt { Key = g.Key, Total = g.Sum(o => o.Amount) })
                .OrderBy(x => x.Key)
                .ToListAsync();
        }

        public async Task<IEnumerable<RevenueGroupInt>> GetRevenueByMonthAsync(int year)
        {
            return await _context.Orders
                .Where(o => o.Status == OrderStatus.Paid && o.CreatedAt.Year == year)
                .GroupBy(o => o.CreatedAt.Month)
                .Select(g => new RevenueGroupInt { Key = g.Key, Total = g.Sum(o => o.Amount) })
                .OrderBy(x => x.Key)
                .ToListAsync();
        }

        public async Task<IEnumerable<RevenueGroupInt>> GetRevenueByDayAsync(int year, int month)
        {
            return await _context.Orders
                .Where(o => o.Status == OrderStatus.Paid && o.CreatedAt.Year == year && o.CreatedAt.Month == month)
                .GroupBy(o => o.CreatedAt.Day)
                .Select(g => new RevenueGroupInt { Key = g.Key, Total = g.Sum(o => o.Amount) })
                .OrderBy(x => x.Key)
                .ToListAsync();
        }

        public async Task<IEnumerable<TopSpender>> GetTopSpendersAsync(DateTime startInclusive, DateTime endExclusive, int top)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Where(o => o.Status == OrderStatus.Paid && o.PaidAt >= startInclusive && o.PaidAt < endExclusive)
                .GroupBy(o => new { o.UserId, o.User.Name, o.User.Email })
                .Select(g => new TopSpender
                {
                    UserId = g.Key.UserId,
                    UserName = g.Key.Name,
                    Email = g.Key.Email ?? string.Empty,
                    Total = g.Sum(o => o.Amount)
                })
                .OrderByDescending(x => x.Total)
                .Take(top)
                .ToListAsync();
        }
    }
}