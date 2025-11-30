using CareerSpark.BusinessLayer.Interfaces;
using Microsoft.Extensions.Logging;

namespace CareerSpark.BusinessLayer.Services
{
    /// <summary>
    /// Service for handling background email jobs with Hangfire
    /// </summary>
    public class EmailBackgroundJobService : IEmailBackgroundJobService
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<EmailBackgroundJobService> _logger;

        public EmailBackgroundJobService(
            IOrderService orderService,
            ILogger<EmailBackgroundJobService> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        /// <summary>
        /// Send order success email in background
        /// </summary>
        public async Task SendOrderSuccessEmailAsync(int orderId)
        {
            try
            {
                _logger.LogInformation("🔥 [Hangfire] Starting background job to send order success email for order {OrderId}", orderId);
                
                var emailSent = await _orderService.SendOrderSuccessEmailAsync(orderId);
                
                if (emailSent)
                {
                    _logger.LogInformation("✅ [Hangfire] Order success email sent successfully for order {OrderId}", orderId);
                }
                else
                {
                    _logger.LogWarning("⚠️ [Hangfire] Failed to send order success email for order {OrderId}", orderId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ [Hangfire] Error sending order success email for order {OrderId}", orderId);
                throw; // Re-throw để Hangfire có thể retry job nếu cần
            }
        }
    }
}
