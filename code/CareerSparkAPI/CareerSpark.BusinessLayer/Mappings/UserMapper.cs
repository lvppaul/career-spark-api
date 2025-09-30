using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.DTOs.Response;
using CareerSpark.BusinessLayer.DTOs.Update;
using CareerSpark.DataAccessLayer.Entities;
using CareerSpark.DataAccessLayer.Enums;

namespace CareerSpark.BusinessLayer.Mappings
{
    public static class UserMapper
    {
        #region Entity to Response
        public static UserResponse ToResponse(this User user)
        {
            if (user == null) return null;
            return new UserResponse
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                Name = user.Name ?? string.Empty,
                Phone = user.Phone,
                CreatedAt = user.CreatedAt,
                IsActive = user.IsActive,
                Role = (UserRole)user.RoleId,
            };
        }
        #endregion

        #region Request to Entity
        public static User ToEntity(this UserRequest request)
        {
            if (request == null) return null;

            return new User
            {
                Email = request.Email?.Trim(),
                Password = request.Password,
                Name = request.Name?.Trim(),
                Phone = request.Phone?.Trim(),
                RoleId = (int)request.RoleId,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
        }
        #endregion

        #region Request to Updated Entity
        public static void ToUpdate(this UserUpdate request, User user)
        {
            if (request == null || user == null) return;

            if (!string.IsNullOrWhiteSpace(request.Email))
                user.Email = request.Email.Trim();

            if (!string.IsNullOrWhiteSpace(request.Name))
                user.Name = request.Name.Trim();

            if (!string.IsNullOrWhiteSpace(request.Phone))
                user.Phone = request.Phone.Trim();
            if (!string.IsNullOrWhiteSpace(request.avatarURL))
                user.avatarURL = request.avatarURL.Trim();

            user.RoleId = (int)request.RoleId;
            user.IsActive = request.IsActive;
        }
        #endregion
    }
}
