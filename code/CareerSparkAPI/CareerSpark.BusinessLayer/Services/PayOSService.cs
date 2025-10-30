using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.DTOs.Response;
using CareerSpark.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Net.payOS;
using Net.payOS.Types;
using System.Net;
using System.Web;

namespace CareerSpark.BusinessLayer.Services
{
    public class PayOSService : IPayOSService
    {
        private readonly PayOS _payOS;
        private readonly IConfiguration _configuration;

        public PayOSService(IConfiguration configuration)
        {
            _configuration = configuration;

            var clientId = _configuration["PayOS:ClientId"] ?? "";
            var apiKey = _configuration["PayOS:ApiKey"] ?? "";
            var checksumKey = _configuration["PayOS:ChecksumKey"] ?? "";

            _payOS = new PayOS(clientId, apiKey, checksumKey);
        }

        public async Task<string> CreatePaymentUrl(PaymentInformationModel model, HttpContext context)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
                ServicePointManager.DefaultConnectionLimit = 100;

                var orderCode = long.Parse($"{model.OrderId}{DateTimeOffset.Now.ToUnixTimeSeconds()}");
                var returnUrl = _configuration["PayOS:ReturnUrl"] ?? "";
                var cancelUrl = _configuration["PayOS:CancelUrl"] ?? "";

                var paymentData = new PaymentData(
                    orderCode: orderCode,
                    amount: (int)(model.Amount),
                    description: model.OrderDescription ?? $"Thanh toán đơn hàng #{model.OrderId}",
                    items: new List<ItemData>
                    {
                        new ItemData(
                            name: model.Name ?? "Subscription Plan",
                            quantity: 1,
                            price: (int)(model.Amount)
                        )
                    },
                    returnUrl: returnUrl,
                    cancelUrl: cancelUrl
                );

                var createPayment = await _payOS.createPaymentLink(paymentData);
                return createPayment.checkoutUrl;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating PayOS payment URL: {ex.Message}", ex);
            }
        }

        public async Task<PaymentResponseModel> PaymentExecute(string linkResponse)
        {
            try
            {
                // Parse query parameters from the response URL
                var uri = new Uri(linkResponse);
                var query = HttpUtility.ParseQueryString(uri.Query);

                var orderCode = query["orderCode"];
                var orderIdString = orderCode?.ToString().Substring(0, orderCode.ToString().Length - 10);
                long orderId = long.Parse(orderIdString);
                var status = query["status"];
                var cancel = query["cancel"];

                if (string.IsNullOrEmpty(orderCode))
                {
                    return new PaymentResponseModel
                    {
                        Success = false,
                        PaymentMethod = "PayOS",
                        TransactionId = "",
                        OrderId = "",
                        PaymentId = "",
                        Token = "",
                        PayOSResponseCode = ""
                    };
                }

                // Get payment information from PayOS
                var paymentInfo = await _payOS.getPaymentLinkInformation(long.Parse(orderCode));

                bool isSuccess = paymentInfo.status == "PAID";

                return new PaymentResponseModel
                {
                    Success = isSuccess,
                    PaymentMethod = "PayOS",
                    OrderId = orderId.ToString(),
                    PaymentId = paymentInfo.id?.ToString() ?? orderCode,
                    TransactionId = paymentInfo.transactions?.FirstOrDefault()?.reference ?? orderCode,
                    Token = paymentInfo.id?.ToString() ?? "",
                    PayOSResponseCode = isSuccess ? "00" : "01",
                    TransactionOrderIdReference = paymentInfo.orderCode
                };
            }
            catch (Exception)
            {
                return new PaymentResponseModel
                {
                    Success = false,
                    PaymentMethod = "PayOS",
                    TransactionId = "",
                    OrderId = "",
                    PaymentId = "",
                    Token = "",
                    PayOSResponseCode = ""
                };
            }
        }
    }
}