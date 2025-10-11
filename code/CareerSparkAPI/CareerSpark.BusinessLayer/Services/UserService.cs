using CareerSpark.BusinessLayer.DTOs.Response;
using CareerSpark.BusinessLayer.DTOs.Update;
using CareerSpark.BusinessLayer.Interfaces;
using CareerSpark.BusinessLayer.Mappings;
using CareerSpark.DataAccessLayer.Entities;
using CareerSpark.DataAccessLayer.Enums;
using CareerSpark.DataAccessLayer.Helper;
using CareerSpark.DataAccessLayer.UnitOfWork;
using Microsoft.AspNetCore.Http;
using System.Net.Mail;

namespace CareerSpark.BusinessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICloudinaryService _cloudinaryService;
        public UserService(IUnitOfWork unitOfWork, ICloudinaryService cloudinaryService)
        {
            _unitOfWork = unitOfWork;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<IEnumerable<UserResponse>> GetAllAsync()
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();
            if (users == null || !users.Any())
                return Enumerable.Empty<UserResponse>();
            return users.Select(UserMapper.ToResponse).ToList();
        }

        public async Task<PaginatedResult<UserResponse>> GetAllAsyncWithPagination(Pagination pagination)
        {
            var result = await _unitOfWork.UserRepository.GetAllAsyncWithPagination(pagination);

            if (result.Items == null || !result.Items.Any())
            {
                return new PaginatedResult<UserResponse>(
                    Enumerable.Empty<UserResponse>(),
                    0,
                    pagination.PageNumber,
                    pagination.PageSize
                );
            }

            var userResponses = result.Items.Select(UserMapper.ToResponse).ToList();

            return new PaginatedResult<UserResponse>(
                userResponses,
                result.TotalCount,
                result.PageNumber,
                result.PageSize
            );
        }

        public async Task<UserResponse> GetByIdAsync(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            return user?.ToResponse();
        }

        public async Task<bool> GetUserByPhoneAsync(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                return false;
            }
            var user = await _unitOfWork.UserRepository.GetByPhoneAsync(phone);
            return user != null;
        }

        public async Task<bool> IsEmailExistsForOtherUserAsync(string email, int currentUserId)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var users = await _unitOfWork.UserRepository.GetAllAsync();
                return users.Any(u => u.Email != null &&
                                     u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
                                     u.Id != currentUserId);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsPhoneExistsForOtherUserAsync(string phone, int currentUserId)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            try
            {
                var existingUser = await _unitOfWork.UserRepository.GetByPhoneAsync(phone);
                return existingUser != null && existingUser.Id != currentUserId;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SetActive(int userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
                return false;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                if (user.IsActive == true)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return true; // User đã active rồi, không cần làm gì thêm
                }
                await _unitOfWork.UserRepository.SetActive(user);
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> Deactive(int userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
                return false;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                if (user.IsActive == true)
                {
                    var userRole = await _unitOfWork.RoleRepository.GetByIdAsync(user.RoleId);
                    // Nếu user hiện tại là Admin
                    if (userRole?.RoleName?.Equals("Admin", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        var allUsers = await _unitOfWork.UserRepository.GetAllAsync();
                        // Đếm ra có bao nhiêu user có role là Admin và đang active, trừ user hiện tại
                        var activeAdmins = allUsers.Where(u => u.IsActive == true && u.Role?.RoleName == "Admin" && u.Id != user.Id).Count();

                        if (activeAdmins == 0)
                        {
                            throw new InvalidOperationException("Cannot deactivate the last active admin user");
                        }
                    }
                }
                else
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return true; // User đã deactive rồi, không cần làm gì thêm
                }
                await _unitOfWork.UserRepository.DeActive(user);
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<UserResponse> UpdateAsync(int id, UserUpdate userUpdate)
        {
            // user truyền vào null hoặc Id <= 0
            if (userUpdate == null)
                throw new ArgumentNullException(nameof(userUpdate), "User update data cannot be null");

            if (id <= 0)
                throw new ArgumentException("Invalid user ID", nameof(id));

            if (!string.IsNullOrWhiteSpace(userUpdate.Email) && !IsValidEmail(userUpdate.Email))
            {
                throw new ArgumentException("Invalid email format", nameof(userUpdate.Email));
            }

            if (!string.IsNullOrWhiteSpace(userUpdate.Phone) && !IsValidPhone(userUpdate.Phone))
            {
                throw new ArgumentException("Invalid phone number format", nameof(userUpdate.Phone));
            }
            // Kiểm tra RoleId có hợp lệ không (trong enum UserRole)
            if (!Enum.IsDefined(typeof(UserRole), userUpdate.RoleId))
            {
                throw new ArgumentException($"Invalid role: {userUpdate.RoleId}", nameof(userUpdate.RoleId));
            }
            else
            {
                // Kiểm tra RoleId có tồn tại trong DB không
                var isValidRole = await IsValidRole((int)userUpdate.RoleId);
                if (!isValidRole)
                {
                    throw new InvalidOperationException($"Role with ID {userUpdate.RoleId} does not exist");
                }
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // Get existing user
                var existingUser = await _unitOfWork.UserRepository.GetByIdAsync(id);
                if (existingUser == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new InvalidOperationException($"User with ID {id} not found");
                }

                // Validate update data
                await ValidateUserUpdate(id, userUpdate, existingUser);

                // Map về lại Entity
                UserMapper.ToUpdate(userUpdate, existingUser);

                var updateResult = await _unitOfWork.UserRepository.UpdateAsync(existingUser);

                if (updateResult == 0)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new InvalidOperationException("Failed to update user information - no changes were saved");
                }

                await _unitOfWork.CommitTransactionAsync();

                // Map về Response
                return UserMapper.ToResponse(existingUser);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhone(string phone)
        {
            if (!string.IsNullOrWhiteSpace(phone) &&
           !System.Text.RegularExpressions.Regex.IsMatch(phone, @"^(0|\+84)(\d{9})$"))
            {
                return false;
            }
            return true;
        }

        private async Task<bool> IsValidRole(int roleId)
        {
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(roleId);
            return role != null;
        }

        private async Task ValidateUserUpdate(int id, UserUpdate userUpdate, User existingUser)
        {
            // Kiểm tra email uniqueness nếu email bị thay đổi
            if (!string.IsNullOrWhiteSpace(userUpdate.Email) &&
                // Email nhập vào không null và khác với email hiện tại
                !userUpdate.Email.Equals(existingUser.Email, StringComparison.OrdinalIgnoreCase))
            {
                // Kiểm tra email đã tồn tại cho user khác chưa
                var emailExists = await IsEmailExistsForOtherUserAsync(userUpdate.Email, id);
                if (emailExists)
                {
                    throw new InvalidOperationException($"Email '{userUpdate.Email}' is already in use ");
                }
            }

            // Kiểm tra phone uniqueness nếu phone bị thay đổi
            if (!string.IsNullOrWhiteSpace(userUpdate.Phone) &&
                // Phone nhập vào không null và khác với phone hiện tại
                !userUpdate.Phone.Equals(existingUser.Phone))
            {
                var phoneExists = await IsPhoneExistsForOtherUserAsync(userUpdate.Phone, id);
                if (phoneExists)
                {
                    throw new InvalidOperationException($"Phone number '{userUpdate.Phone}' is already in use");
                }
            }

            // Kiểm tra RoleId nhập vào có tồn tại không nếu bị thay đổi
            if ((int)userUpdate.RoleId != existingUser.RoleId)
            {
                var role = await _unitOfWork.RoleRepository.GetByIdAsync((int)userUpdate.RoleId);
                if (role == null)
                {
                    throw new InvalidOperationException($"Role with ID {userUpdate.RoleId} does not exist");
                }
            }

            // Ngăn ngừa việc vô hiệu hóa user Admin cuối cùng
            // IsActive update có giá trị là false và IsActive hiện tại là true
            if (!userUpdate.IsActive && existingUser.IsActive == true)
            {
                var userRole = await _unitOfWork.RoleRepository.GetByIdAsync(existingUser.RoleId);
                // Nếu user hiện tại là Admin
                if (userRole?.RoleName?.Equals("Admin", StringComparison.OrdinalIgnoreCase) == true)
                {
                    var allUsers = await _unitOfWork.UserRepository.GetAllAsync();
                    // Đếm ra có bao nhiêu user có role là Admin và đang active, trừ user hiện tại
                    var activeAdmins = allUsers.Where(u => u.IsActive == true && u.Role?.RoleName == "Admin" && u.Id != existingUser.Id).Count();

                    if (activeAdmins == 0)
                    {
                        throw new InvalidOperationException("Cannot deactivate the last active admin user");
                    }
                }
            }

        }

        public async Task<bool> UpdateAvatar(int userId, IFormFile avatarImage)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
                return false;
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                // Validate file
                var (isValid, errorMessage) = _cloudinaryService.ValidateDocumentFile(avatarImage);
                if (!isValid)
                {
                    throw new InvalidOperationException("Wrong Image Extension");
                }
                // Upload new avatar
                var avatarUploadResult = await _cloudinaryService.UploadFileAsync(avatarImage, "AvatarImages");
                if (avatarUploadResult == null || string.IsNullOrWhiteSpace(avatarUploadResult.PublicId))
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new InvalidOperationException("Avatar upload failed");
                }
                // Xoá avatar cũ trên Cloudinary nếu có
                if (!string.IsNullOrWhiteSpace(user.avatarPublicId))
                {
                    var deleteResult = await _cloudinaryService.DeleteFileAsync(user.avatarPublicId);
                    if (!deleteResult)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        throw new InvalidOperationException("Failed to delete old avatar from Cloudinary");
                    }
                }
                // Cập nhật avatar mới cho user
                user.avatarURL = avatarUploadResult.SecureUrl;
                user.avatarPublicId = avatarUploadResult.PublicId;

                var updateResult = await _unitOfWork.UserRepository.UpdateAsync(user);
                if (updateResult == 0)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new InvalidOperationException("Failed to update user avatar - no changes were saved");
                }
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
