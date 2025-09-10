using CareerSpark.DataAccessLayer.Entities;

namespace CareerSpark.DataAccessLayer.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task SetActiveOrDeactive(User user);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByRefreshTokenAsync(string refreshToken);
        Task<bool> VerifyPasswordAsync(User user, string password);

        Task<User> GetByPhoneAsync(string phone);

        //    Task<List<User>> GetAllAsyncWithPagination(Pagination pagination);
    }
}
