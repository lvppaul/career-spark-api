using CareerSpark.BusinessLayer.Interfaces;
using CareerSpark.DataAccessLayer.UnitOfWork;

namespace CareerSpark.BusinessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> GetUserByPhoneAsync(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                return false;
            }
            var user = await _unitOfWork.UserRepository.GetByPhoneAsync(phone);
            if (user == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> SetActive(int userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
                return false;
            try
            {
                await _unitOfWork.UserRepository.SetActive(user);
                await _unitOfWork.CommitTransactionAsync();
                return true;

            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                Console.WriteLine($"Error deleting user: {ex.Message}");
                throw;
            }

        }


    }
}
