using CareerSpark.BusinessLayer.DTOs.Response;
using CareerSpark.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Net.payOS.Types;
using Newtonsoft.Json;

namespace CareerSpark.API.Controllers
{
    [ApiController]
    [Route("api/payos/webhook")]
    public class PayOSWebhookController : ControllerBase
    {
        private readonly ILogger<PayOSWebhookController> _logger;
        private readonly PayOS _payOS;
        private readonly IPayOSService _payOSService;
        private readonly IOrderService _orderService;

        public PayOSWebhookController(
            ILogger<PayOSWebhookController> logger,
            PayOS payOS,
            IPayOSService payOSService,
            IOrderService orderService)
        {
            _logger = logger;
            _payOS = payOS;
            _payOSService = payOSService;
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> ReceiveWebhook([FromBody] WebhookType webhookBody)
        {
            try
            {
                _logger.LogInformation("🔔 PayOS webhook received: {Payload}", JsonConvert.SerializeObject(webhookBody));

                // ✅ Xác minh chữ ký và lấy dữ liệu thực
                var data = _payOS.verifyPaymentWebhookData(webhookBody);
                if (data == null)
                {
                    _logger.LogWarning("Webhook verification failed (data is null)");
                    return BadRequest(new { code = "01", message = "Invalid signature or data" });
                }

                var orderIdString = data.orderCode.ToString().Substring(0, data.orderCode.ToString().Length - 10);
                // 🧾 Chuyển dữ liệu webhook sang PaymentResponseModel
                var paymentResponse = new PaymentResponseModel
                {
                    Success = webhookBody.success && webhookBody.code == "00",
                    PaymentMethod = "PayOS",
                    OrderId = orderIdString,
                    PaymentId = data.paymentLinkId,
                    TransactionId = data.reference,
                    Token = data.description,
                    PayOSResponseCode = webhookBody.code,
                    TransactionOrderIdReference = data.orderCode
                };

                // 🧠 Gọi xử lý trong OrderService
                var processed = await _orderService.ProcessPaymentCallbackAsync(paymentResponse);

                if (!processed)
                {
                    _logger.LogWarning("❌ Failed to process webhook for order {OrderCode}", data.orderCode);
                    return Ok(new { code = "01", message = "Process failed" });
                }

                _logger.LogInformation("✅ Webhook processed successfully for order {OrderCode}", data.orderCode);
                return Ok(new { code = "00", message = "success" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "🚨 Error processing PayOS webhook");
                return StatusCode(500, new { code = "99", message = "Internal server error" });
            }
        }
    }
}
