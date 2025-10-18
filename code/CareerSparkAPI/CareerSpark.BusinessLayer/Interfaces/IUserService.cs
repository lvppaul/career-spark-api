using CareerSpark.BusinessLayer.DTOs.Response;
using CareerSpark.BusinessLayer.DTOs.Update;
using CareerSpark.DataAccessLayer.Helper;
using Microsoft.AspNetCore.Http;

namespace CareerSpark.BusinessLayer.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponse>> GetAllAsync();
        Task<PaginatedResult<UserResponse>> GetAllAsyncWithPagination(Pagination pagination);
        Task<UserResponse> GetByIdAsync(int id);
        Task<bool> GetUserByPhoneAsync(string phone);
        Task<bool> IsEmailExistsForOtherUserAsync(string email, int currentUserId);
        Task<bool> IsPhoneExistsForOtherUserAsync(string phone, int currentUserId);
        Task<bool> SetActive(int userId);
        Task<bool> Deactive(int userId);
        Task<UserResponse> UpdateAsync(int id, UserUpdate user);

        Task<bool> UpdateAvatar(int userId, IFormFile avatarImage);
        Task<bool> UpdatePasswordAsync(int userId, PasswordUpdate passwordUpdate);
    }
}
