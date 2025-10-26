using CareerSpark.DataAccessLayer.Context;
using CareerSpark.DataAccessLayer.Entities;
using CareerSpark.DataAccessLayer.Helper;
using CareerSpark.DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CareerSpark.DataAccessLayer.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(CareerSparkDbContext context) : base(context)
        {
        }

        public async Task SetActive(User user)
        {
            // user.IsActive = !user.IsActive;
            user.IsActive = true;
            await UpdateAsync(user);
        }

        public async Task DeActive(User user)
        {
            user.IsActive = false;
            await UpdateAsync(user);
        }

        public override async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public override async Task<List<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .ToListAsync();
        }

        public override async Task<PaginatedResult<User>> GetAllAsyncWithPagination(Pagination pagination)
        {
            // Get total count
            var totalCount = await _context.Users.CountAsync();

            // Get paginated items with Role included
            var items = await _context.Users
                .Include(u => u.Role)
                .OrderBy(u => u.Id) // Thêm ordering để đảm bảo consistent pagination
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();

            return new PaginatedResult<User>(items, totalCount, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            return await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                return null;

            return await _context.Users
       .Include(u => u.Role)
       .FirstOrDefaultAsync(u =>
           u.RefreshToken == refreshToken &&
           u.IsActive == true);

        }

        public async Task<bool> VerifyPasswordAsync(User user, string password)
        {
            return await Task.FromResult(BCrypt.Net.BCrypt.Verify(password, user.Password));
        }

        public async Task<User> GetByPhoneAsync(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return null;

            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Phone == phone);
        }

        public async Task<bool> IsEmailVerifiedAsync(int userId)
        {
            var result = await _context.Users
                .AnyAsync(u => u.Id == userId && u.IsVerified);

            return result;
        }
    }
}
