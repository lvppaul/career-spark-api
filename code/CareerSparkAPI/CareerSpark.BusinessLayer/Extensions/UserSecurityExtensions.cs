using CareerSpark.DataAccessLayer.Entities;

namespace CareerSpark.BusinessLayer.Extensions
{
    public static class UserSecurityExtensions
    {
        /// <summary>
        /// Tạo SecurityStamp mới
        /// </summary>
        public static void UpdateSecurityStamp(this User user)
        {
            user.SecurityStamp = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Kiểm tra SecurityStamp có khớp không
        /// </summary>
        public static bool IsSecurityStampValid(this User user, string tokenSecurityStamp)
        {
            return user.SecurityStamp.Equals(tokenSecurityStamp, StringComparison.Ordinal);
        }

        /// <summary>
        /// Xóa refresh token và buộc user phải đăng nhập lại
        /// Sử dụng trong các trường hợp cần bảo mật cao như:
        /// - Thay đổi mật khẩu
        /// - Thay đổi email
        /// - Thay đổi thông tin quan trọng
        /// - Phát hiện hoạt động đáng ngờ
        /// </summary>
        public static void InvalidateRefreshToken(this User user)
        {
            user.RefreshToken = null;
            user.ExpiredRefreshTokenAt = null;
        }

        /// <summary>
        /// Cập nhật SecurityStamp và xóa refresh token cùng lúc
        /// Sử dụng khi cần invalidate tất cả token và buộc đăng nhập lại
        /// </summary>
        public static void InvalidateAllTokens(this User user)
        {
            user.UpdateSecurityStamp();
            user.InvalidateRefreshToken();
        }
    }
}