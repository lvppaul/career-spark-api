﻿namespace CareerSpark.BusinessLayer.Libraries
{
    public static class EmailTemplateReader
    {
        public static string ConfirmationTemplate(string userName, string confirmUrl)
        {
            return $@"
        <html>
        <body style='font-family: Arial;'>
            <h2>Xin chào {userName},</h2>
            <img src='https://res.cloudinary.com/dliirxsmo/image/upload/v1761800009/logo_dacig0.jpg' alt='Logo' width='120'/>
            <p>Cảm ơn bạn đã đăng ký tài khoản tại <b>CareerSpark</b>.</p>
            <p>Vui lòng xác nhận email của bạn bằng cách nhấn nút bên dưới:</p>
            <p>
                <a href='{confirmUrl}' 
                   style='background-color: #007bff; color: white; padding: 10px 20px; 
                          text-decoration: none; border-radius: 5px;'>Xác nhận Email</a>
            </p>
            <p>Nếu bạn không đăng ký, hãy bỏ qua email này.</p>
        </body>
        </html>";
        }
    }
}
