namespace CareerSpark.BusinessLayer.Interfaces
{
    public interface IUserService
    {
        Task<bool> SetActive(int userId);
        Task<bool> GetUserByPhoneAsync(string phone);
    }
}
