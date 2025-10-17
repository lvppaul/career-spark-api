using CareerSpark.BusinessLayer.DTOs.Request;

namespace CareerSpark.BusinessLayer.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailRequest emailRequest, CancellationToken cancellationToken);
    }
}
