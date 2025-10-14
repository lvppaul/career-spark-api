namespace CareerSpark.BusinessLayer.Libraries
{
    public static class EmailResetPasswordTemplate
    {
        public static string ResetConfirmationTemplate(string userName, string confirmUrl)
        {
            return $@"
        <html>
        <body style='font-family: Arial;'>
            <h2>Xin chào {userName},</h2>
            <p>Cảm ơn bạn đã đăng ký tài khoản tại <b>CareerSpark</b>.</p>
            <p>Vui lòng nhấn nút bên dưới để có thể reset password:</p>
            <p>
                <a href='{confirmUrl}' 
                   style='background-color: #007bff; color: white; padding: 10px 20px; 
                          text-decoration: none; border-radius: 5px;'>Reset password</a>
            </p>
            <p>Nếu bạn không đăng ký, hãy bỏ qua email này.</p>
        </body>
        </html>";
        }
    }
}
