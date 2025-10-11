using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CareerSpark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;
        private readonly IOrderService _orderService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IVnPayService vnPayService, IOrderService orderService, IConfiguration configuration, ILogger<PaymentController> logger)
        {
            _vnPayService = vnPayService;
            _orderService = orderService;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("CreatePaymentUrlVnpay")]
        public async Task<IActionResult> CreatePaymentUrlVnpay(PaymentInformationModel model)
        {
            try
            {
                var url = await _vnPayService.CreatePaymentUrl(model, HttpContext);
                if (string.IsNullOrEmpty(url))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Cannot create payment url",
                        timestamp = DateTime.UtcNow
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Payment URL created successfully",
                    paymentUrl = url,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error creating payment URL",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        // -------------------- Xử lý callback từ VNPay --------------------
        [HttpGet("Checkout/PaymentCallbackVnpay")]
        public async Task<IActionResult> PaymentCallbackVnpay()
        {
            try
            {
                // 1. Lấy toàn bộ query mà VNPay gửi về
                var query = HttpContext.Request.Query;
                var rawUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}?{Request.QueryString}";

                // 2. Parse và xác minh chữ ký
                var vnpayResponse = await _vnPayService.PaymentExecute(rawUrl);
                if (vnpayResponse == null)
                    return Redirect(BuildFrontendUrl("failed", "invalid_signature"));

                // 3. Cập nhật Order trong DB
                var callbackProcessed = await _orderService.ProcessPaymentCallbackAsync(vnpayResponse);
                if (!callbackProcessed)
                    return Redirect(BuildFrontendUrl("failed", "process_failed", vnpayResponse.OrderId));

                // 4. Redirect sang FE tuỳ kết quả
                if (vnpayResponse.Success && vnpayResponse.VnPayResponseCode == "00")
                {
                    return Redirect(BuildFrontendUrl(
                        "success",
                        "payment_success",
                        vnpayResponse.OrderId,
                        vnpayResponse.TransactionOrderIdReference,
                        vnpayResponse.VnPayResponseCode
                    ));
                }
                else
                {
                    return Redirect(BuildFrontendUrl(
                        "failed",
                        "payment_failed",
                        vnpayResponse.OrderId,
                        vnpayResponse.TransactionOrderIdReference,
                        vnpayResponse.VnPayResponseCode
                    ));
                }
            }
            catch (Exception ex)
            {
                return Redirect(BuildFrontendUrl("failed", Uri.EscapeDataString(ex.Message)));
            }
        }


        private string BuildFrontendUrl(
          string status,
          string message,
          string? orderId = null,
          double? transactionRef = null,
          string? code = null)
        {
            // FE base URL: chỉ cần domain (không bao gồm /payment/result)
            var feBaseUrl = _configuration["Frontend:BaseUrl"] ?? "http://localhost:5173";
            var url = $"{feBaseUrl}/payment/result?status={status}&message={message}";

            if (!string.IsNullOrEmpty(orderId))
                url += $"&orderId={orderId}";
            if (transactionRef.HasValue)
                url += $"&txnRef={transactionRef}";
            if (!string.IsNullOrEmpty(code))
                url += $"&code={code}";

            return url;
        }

    }
}
