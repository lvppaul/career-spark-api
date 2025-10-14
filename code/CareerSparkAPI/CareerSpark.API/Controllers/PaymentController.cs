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

        public PaymentController(IVnPayService vnPayService, IOrderService orderService)
        {
            _vnPayService = vnPayService;
            _orderService = orderService;
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

        [HttpGet("Checkout/PaymentCallbackVnpay")]
        public async Task<IActionResult> PaymentCallbackVnpay(string linkResponse)
        {
            try
            {
                if (string.IsNullOrEmpty(linkResponse))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid callback data",
                        timestamp = DateTime.UtcNow
                    });
                }

                // Process VNPay response
                var vnpayResponse = await _vnPayService.PaymentExecute(linkResponse);

                if (vnpayResponse == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Failed to process VNPay response",
                        timestamp = DateTime.UtcNow
                    });
                }

                // Process payment callback to update order and create subscription
                var callbackProcessed = await _orderService.ProcessPaymentCallbackAsync(vnpayResponse);

                if (!callbackProcessed)
                {
                    return StatusCode(500, new
                    {
                        success = false,
                        message = "Failed to process payment callback",
                        vnpayResponse = vnpayResponse,
                        timestamp = DateTime.UtcNow
                    });
                }

                // Return success response based on VNPay result
                if (vnpayResponse.Success && vnpayResponse.VnPayResponseCode == "00")
                {
                    return Ok(new
                    {
                        success = true,
                        message = "Payment processed successfully",
                        data = vnpayResponse,
                        timestamp = DateTime.UtcNow
                    });
                }
                else
                {
                    return Ok(new
                    {
                        success = false,
                        message = "Payment failed or was cancelled",
                        data = vnpayResponse,
                        timestamp = DateTime.UtcNow
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error processing payment callback",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }
    }
}
