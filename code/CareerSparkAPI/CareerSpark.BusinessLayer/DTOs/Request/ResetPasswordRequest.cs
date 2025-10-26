﻿namespace CareerSpark.BusinessLayer.DTOs.Request
{
    public class ResetPasswordRequest
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }

        public string ConfirmNewPassword { get; set; }
    }
}
