namespace CareerSpark.BusinessLayer.Interfaces
{
    /// <summary>
    /// Interface for handling background email jobs
    /// </summary>
    public interface IEmailBackgroundJobService
    {
        /// <summary>
        /// Send order success email in background
        /// </summary>
        /// <param name="orderId">Order ID to send email for</param>
        Task SendOrderSuccessEmailAsync(int orderId);
    }
}
