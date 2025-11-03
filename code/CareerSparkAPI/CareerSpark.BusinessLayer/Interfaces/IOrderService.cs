using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.DTOs.Response;
using Microsoft.AspNetCore.Http;

namespace CareerSpark.BusinessLayer.Interfaces
{
    public interface IOrderService
    {
        Task<CreateOrderResponse> CreateOrderAsync(CreateOrderRequest request, HttpContext httpContext);
        Task<OrderResponse?> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<OrderResponse>> GetOrdersByUserIdAsync(int userId);
        Task<bool> ProcessPaymentCallbackAsync(PaymentResponseModel paymentResponse);
        Task<bool> CancelExpiredOrdersAsync();
        Task<PaginatedOrderResponse> GetOrdersPagedAsync(int pageNumber, int pageSize, int? year, int? month, int? day);

        // Revenue
        Task<decimal> GetTotalRevenueAsync(DateTime? start, DateTime? end);
        Task<IEnumerable<KeyValuePair<int, decimal>>> GetRevenueByYearAsync();
        Task<IEnumerable<KeyValuePair<int, decimal>>> GetRevenueByMonthAsync(int year);
        Task<IEnumerable<KeyValuePair<int, decimal>>> GetRevenueByDayAsync(int year, int month);
    }

    public class PaginatedOrderResponse
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