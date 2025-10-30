using CareerSpark.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CareerSpark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPayOSService _payOSService;
        private readonly IOrderService _orderService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPayOSService payOSService, IOrderService orderService, IConfiguration configuration, ILogger<PaymentController> logger)
        {
            _payOSService = payOSService;
            _orderService = orderService;
            _configuration = configuration;
            _logger = logger;
        }

        //[HttpPost("CreatePaymentUrlVnpay")]
        //public async Task<IActionResult> CreatePaymentUrlVnpay(PaymentInformationModel model)
        //{
        //    try
        //    {
        //        var url = await _vnPayService.CreatePaymentUrl(model, HttpContext);
        //        if (string.IsNullOrEmpty(url))
        //        {
        //            return BadRequest(new
        //            {
        //                success = false,
        //                message = "Cannot create payment url",
        //                timestamp = DateTime.UtcNow
        //            });
        //        }

        //        return Ok(new
        //        {
        //            success = true,
        //            message = "Payment URL created successfully",
        //            paymentUrl = url,
        //            timestamp = DateTime.UtcNow
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new
        //        {
        //            success = false,
        //            message = "Error creating payment URL",
        //            error = ex.Message,
        //            timestamp = DateTime.UtcNow
        //        });
        //    }
        //}

        // -------------------- Xử lý callback từ VNPay --------------------
        [HttpGet("Checkout/PaymentCallbackPayOS")]
        public async Task<IActionResult> PaymentCallbackVnpay()
        {
            try
            {
                // 1. Lấy toàn bộ query mà PayOS gửi về
                var query = HttpContext.Request.Query;
                var rawUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}?{Request.QueryString}";

                // 2. Parse và xác minh thanh toán
                var payOSResponse = await _payOSService.PaymentExecute(rawUrl);
                if (payOSResponse == null)
                    return Redirect(BuildFrontendUrl("failed", "invalid_response"));

                // 3. Cập nhật Order trong DB
                var callbackProcessed = await _orderService.ProcessPaymentCallbackAsync(payOSResponse);
                if (!callbackProcessed)
                    return Redirect(BuildFrontendUrl("failed", "process_failed", payOSResponse.OrderId));

                // 4. Redirect sang FE tuỳ kết quả
                if (payOSResponse.Success && payOSResponse.PayOSResponseCode == "00")
                {
                    return Redirect(BuildFrontendUrl(
                        "success",
                        "payment_success",
                        payOSResponse.OrderId,
                        payOSResponse.TransactionOrderIdReference,
                        payOSResponse.PayOSResponseCode
                    ));
                }
                else
                {
                    return Redirect(BuildFrontendUrl(
                        "failed",
                        "payment_failed",
                        payOSResponse.OrderId,
                        payOSResponse.TransactionOrderIdReference,
                        payOSResponse.PayOSResponseCode
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
            var feBaseUrl = _configuration["Frontend:BaseUrl"] ?? "https://localhost:5173";
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
