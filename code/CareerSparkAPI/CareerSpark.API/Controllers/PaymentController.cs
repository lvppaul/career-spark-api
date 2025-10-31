using CareerSpark.BusinessLayer.DTOs.Response;
using CareerSpark.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Net.payOS.Types;
using Newtonsoft.Json;

namespace CareerSpark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPayOSService _payOSService;
        private readonly PayOS _payOS;
        private readonly IOrderService _orderService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPayOSService payOSService, IOrderService orderService, IConfiguration configuration, ILogger<PaymentController> logger)
        {
            _payOSService = payOSService;
            _orderService = orderService;
            _configuration = configuration;
            _logger = logger;
            var clientId = _configuration["PayOS:ClientId"] ?? "";
            var apiKey = _configuration["PayOS:ApiKey"] ?? "";
            var checksumKey = _configuration["PayOS:ChecksumKey"] ?? "";
            _payOS = new PayOS(clientId, apiKey, checksumKey);
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

        // -------------------- Xử lý callback từ PayOS --------------------
        //[HttpGet("Checkout/PaymentCallbackPayOS")]
        //public async Task<IActionResult> PaymentCallbackPayOS()
        //{
        //    try
        //    {
        //        // 1️ Lấy URL callback đầy đủ từ PayOS
        //        var rawUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}?{Request.QueryString}";

        //        // 2️ Gọi PayOSService để xác minh thanh toán
        //        var payOSResponse = await _payOSService.PaymentExecute(rawUrl);
        //        if (payOSResponse == null)
        //            return Redirect(BuildFrontendUrl("failed", "invalid_response"));

        //        // 3️ Cập nhật Order trong DB
        //        var processed = await _orderService.ProcessPaymentCallbackAsync(payOSResponse);
        //        if (!processed)
        //            return Redirect(BuildFrontendUrl("failed", "process_failed", payOSResponse.OrderId));

        //        // 4️ Redirect sang FE theo kết quả thanh toán
        //        if (payOSResponse.Success && payOSResponse.PayOSResponseCode == "00")
        //        {
        //            return Redirect(BuildFrontendUrl(
        //                "success",
        //                "payment_success",
        //                payOSResponse.OrderId
        //            ));
        //        }
        //        else
        //        {
        //            return Redirect(BuildFrontendUrl(
        //                "failed",
        //                "payment_failed",
        //                payOSResponse.OrderId
        //            ));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Redirect(BuildFrontendUrl("failed", Uri.EscapeDataString(ex.Message)));
        //    }
        //}



        //private string BuildFrontendUrl(
        //     string status,
        //     string message,
        //     string? orderId = null,
        //     double? transactionRef = null,
        //     string? code = null)
        //{
        //    var feBaseUrl = _configuration["Frontend:BaseUrl"] ?? "https://localhost:5173";
        //    var url = $"{feBaseUrl}/payment/result?status={status}&message={message}";

        //    if (!string.IsNullOrEmpty(orderId))
        //        url += $"&orderId={orderId}";
        //    if (transactionRef.HasValue)
        //        url += $"&txnRef={transactionRef}";
        //    if (!string.IsNullOrEmpty(code))
        //        url += $"&code={code}";

        //    return url;
        //}

        [HttpPost("Handle-Webhook")]
        public async Task<IActionResult> ReceiveWebhook([FromBody] WebhookType webhookBody)
        {
            try
            {
                _logger.LogInformation(" PayOS webhook received: {Payload}", JsonConvert.SerializeObject(webhookBody));

                //  Xác minh chữ ký và lấy dữ liệu thực
                var data = _payOS.verifyPaymentWebhookData(webhookBody);
                if (data == null)
                {
                    _logger.LogWarning("Webhook verification failed (data is null)");
                    return BadRequest(new { code = "01", message = "Invalid signature or data" });
                }

                var orderIdString = data.orderCode.ToString().Substring(0, data.orderCode.ToString().Length - 10);
                //  Chuyển dữ liệu webhook sang PaymentResponseModel
                var paymentResponse = new PaymentResponseModel
                {
                    Success = webhookBody.success && webhookBody.code == "00",
                    PaymentMethod = "PayOS",
                    OrderId = orderIdString,
                    PaymentId = data.paymentLinkId,
                    TransactionId = data.reference,
                    Token = data.paymentLinkId,
                    PayOSResponseCode = webhookBody.code,
                    TransactionOrderIdReference = data.orderCode
                };

                //  Gọi xử lý trong OrderService
                var processed = await _orderService.ProcessPaymentCallbackAsync(paymentResponse);

                if (!processed)
                {
                    _logger.LogWarning(" Failed to process webhook for order {OrderCode}", data.orderCode);
                    return Ok(new { code = "01", message = "Process failed" });
                }

                _logger.LogInformation(" Webhook processed successfully for order {OrderCode}", data.orderCode);
                return Ok(new { code = "00", message = "success" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, " Error processing PayOS webhook");
                return StatusCode(500, new { code = "99", message = "Internal server error" });
            }
        }

    }
}
