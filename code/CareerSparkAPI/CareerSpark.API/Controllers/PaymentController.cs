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

        // -------------------- Xử lý callback từ PayOS --------------------
        [HttpGet("Checkout/PaymentCallbackPayOS")]
        public async Task<IActionResult> PaymentCallbackPayOS()
        {
            try
            {
                var rawUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}?{Request.QueryString}";
                var payOSResponse = await _payOSService.PaymentExecute(rawUrl);

                if (payOSResponse == null)
                    return RedirectPermanent(BuildFrontendUrl("failed", "invalid_response"));

                var processed = await _orderService.ProcessPaymentCallbackAsync(payOSResponse);
                if (!processed)
                    return RedirectPermanent(BuildFrontendUrl("failed", "process_failed", payOSResponse.OrderId));

                if (payOSResponse.Success && payOSResponse.PayOSResponseCode == "00")
                    return RedirectPermanent(BuildFrontendUrl("success", "payment_success", payOSResponse.OrderId));
                else
                    return RedirectPermanent(BuildFrontendUrl("failed", "payment_failed", payOSResponse.OrderId));
            }
            catch (Exception ex)
            {
                return RedirectPermanent(BuildFrontendUrl("failed", Uri.EscapeDataString(ex.Message)));
            }
        }


        private string BuildFrontendUrl(
             string status,
             string message,
             string? orderId = null,
             double? transactionRef = null,
             string? code = null)
        {
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
