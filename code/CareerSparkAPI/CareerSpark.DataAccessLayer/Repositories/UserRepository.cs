using CareerSpark.DataAccessLayer.Context;
using CareerSpark.DataAccessLayer.Entities;
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
            user.IsActive = !user.IsActive;
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
            return await _context.Users.FirstOrDefaultAsync(u => u.Phone == phone);
        }
    }
}
