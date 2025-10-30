namespace CareerSpark.BusinessLayer.DTOs.Response
{
    public class PaymentResponseModel
    {
        public string TransactionId { get; set; }
        public string OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentId { get; set; }
        public bool Success { get; set; }
        public string Token { get; set; }
        public string PayOSResponseCode { get; set; }
        public double TransactionOrderIdReference { get; set; }

    }
}
