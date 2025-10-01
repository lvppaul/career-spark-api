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
    }
}