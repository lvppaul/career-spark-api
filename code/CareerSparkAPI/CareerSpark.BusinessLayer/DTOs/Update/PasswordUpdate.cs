namespace CareerSpark.BusinessLayer.DTOs.Update
{
    public class PasswordUpdate
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
