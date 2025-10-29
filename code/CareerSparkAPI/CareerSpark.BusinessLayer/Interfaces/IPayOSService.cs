using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.DTOs.Response;
using Microsoft.AspNetCore.Http;

namespace CareerSpark.BusinessLayer.Interfaces
{
    public interface IPayOSService
    {
        Task<string> CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
        Task<PaymentResponseModel> PaymentExecute(string linkResponse);
    }
}