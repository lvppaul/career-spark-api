using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.Interfaces;
using Microsoft.Extensions.Configuration;
using Resend;

namespace CareerSpark.BusinessLayer.Services
{
    public class ResendEmailService : IEmailService
    {
        private readonly IResend _resend;
        private readonly IConfiguration _configuration;
        private readonly string _fromEmail;
        private readonly string _fromName;
        public ResendEmailService(IConfiguration configuration, IResend resend)
        {
            _configuration = configuration;
            _fromEmail = _configuration["Resend:FromEmail"];
            _fromName = _configuration["Resend:FromName"];
            _resend = resend;
        }
        public async Task SendEmailAsync(EmailRequest emailRequest, CancellationToken cancellationToken)
        {
            try
            {
                var message = new EmailMessage();
                message.From = $"{_fromName} <{_fromEmail}>";
                message.To.Add(emailRequest.To);
                message.Subject = emailRequest.Subject;
                message.HtmlBody = emailRequest.Body;

                // Initialize Attachments collection
                message.Attachments = new List<EmailAttachment>();

                // Handle attachments if any
                if (emailRequest.AttachmentFilePaths?.Length > 0)
                {
                    foreach (var filePath in emailRequest.AttachmentFilePaths)
                    {
                        if (File.Exists(filePath))
                        {
                            var fileName = Path.GetFileName(filePath);
                            var fileBytes = await File.ReadAllBytesAsync(filePath, cancellationToken);
                            var base64Data = Convert.ToBase64String(fileBytes);

                            // Tạo attachment theo chuẩn Resend
                            message.Attachments.Add(new EmailAttachment
                            {
                                Filename = fileName,
                                Content = base64Data,
                            });
                        }

                    }
                }
                else
                {
                    message.Attachments.Add(new EmailAttachment
                    {
                        Filename = "CareerSpark Logo",
                        Path = "https://res.cloudinary.com/dliirxsmo/image/upload/v1761800009/logo_dacig0.jpg",
                        ContentType = "image/jpg"
                    });
                }

                var response = await _resend.EmailSendAsync(message, cancellationToken);

                if (!response.Success)
                {
                    throw new Exception($"Failed to send email via Resend: {response.Exception.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error sending email via Resend: {ex.Message}", ex);
            }
        }
    }
}
