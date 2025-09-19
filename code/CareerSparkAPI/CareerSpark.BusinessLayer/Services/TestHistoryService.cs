using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.DTOs.Response;
using CareerSpark.BusinessLayer.DTOs.Update;
using CareerSpark.BusinessLayer.Interfaces;
using CareerSpark.BusinessLayer.Mappings;
using CareerSpark.DataAccessLayer.Helper;
using CareerSpark.DataAccessLayer.UnitOfWork;

namespace CareerSpark.BusinessLayer.Services
{
    public class TestHistoryService : ITestHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TestHistoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TestHistoryResponse>> GetAllAsync()
        {
            var items = await _unitOfWork.TestHistoryRepository.GetAllAsync();
            if (items == null || !items.Any()) return Enumerable.Empty<TestHistoryResponse>();
            return items.Select(TestHistoryMapper.ToResponse).ToList();
        }

        public async Task<PaginatedResult<TestHistoryResponse>> GetAllAsyncWithPagination(Pagination pagination)
        {
            var result = await _unitOfWork.TestHistoryRepository.GetAllAsyncWithPagination(pagination);
            if (result.Items == null || !result.Items.Any())
            {
                return new PaginatedResult<TestHistoryResponse>(Enumerable.Empty<TestHistoryResponse>(), 0, pagination.PageNumber, pagination.PageSize);
            }

            var mapped = result.Items.Select(TestHistoryMapper.ToResponse).ToList();
            return new PaginatedResult<TestHistoryResponse>(mapped, result.TotalCount, result.PageNumber, result.PageSize);
        }

        public async Task<TestHistoryResponse?> GetByIdAsync(int id)
        {
            if (id <= 0) return null;
            var entity = await _unitOfWork.TestHistoryRepository.GetByIdAsync(id);
            return entity?.ToResponse();
        }

        public async Task<IEnumerable<TestHistoryResponse>> GetByUserIdAsync(int userId)
        {
            if (userId <= 0) return Enumerable.Empty<TestHistoryResponse>();
            var all = await _unitOfWork.TestHistoryRepository.GetAllAsync();
            var filtered = all.Where(x => x.UserId == userId);
            return filtered.Select(TestHistoryMapper.ToResponse).ToList();
        }

        public async Task<TestHistoryResponse> CreateAsync(TestHistoryRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (request.UserId <= 0) throw new ArgumentException("Invalid user id", nameof(request.UserId));
            if (request.ResultId <= 0) throw new ArgumentException("Invalid result id", nameof(request.ResultId));
            if (request.TestAnswerId <= 0) throw new ArgumentException("Invalid test answer id", nameof(request.TestAnswerId));

            // ensure related entities exist
            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);
            if (user == null) throw new InvalidOperationException($"User with id {request.UserId} not found");
            var result = await _unitOfWork.ResultRepository.GetByIdAsync(request.ResultId);
            if (result == null) throw new InvalidOperationException($"Result with id {request.ResultId} not found");
            var testAnswer = await _unitOfWork.TestAnswerRepository.GetByIdAsync(request.TestAnswerId);
            if (testAnswer == null) throw new InvalidOperationException($"TestAnswer with id {request.TestAnswerId} not found");

            var entity = request.ToEntity();

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var res = await _unitOfWork.TestHistoryRepository.CreateAsync(entity);
                if (res <= 0)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new InvalidOperationException("Failed to create test history");
                }

                await _unitOfWork.CommitTransactionAsync();
                return entity.ToResponse();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<TestHistoryResponse> UpdateAsync(int id, TestHistoryUpdate update)
        {
            if (id <= 0) throw new ArgumentException("Invalid id", nameof(id));
            if (update == null) throw new ArgumentNullException(nameof(update));

            var entity = await _unitOfWork.TestHistoryRepository.GetByIdAsync(id);
            if (entity == null) throw new InvalidOperationException($"TestHistory with id {id} not found");

            // validate related entities if provided
            if (update.ResultId.HasValue)
            {
                var result = await _unitOfWork.ResultRepository.GetByIdAsync(update.ResultId.Value);
                if (result == null) throw new InvalidOperationException($"Result with id {update.ResultId.Value} not found");
            }
            if (update.TestAnswerId.HasValue)
            {
                var answer = await _unitOfWork.TestAnswerRepository.GetByIdAsync(update.TestAnswerId.Value);
                if (answer == null) throw new InvalidOperationException($"TestAnswer with id {update.TestAnswerId.Value} not found");
            }

            update.ToUpdate(entity);

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var res = await _unitOfWork.TestHistoryRepository.UpdateAsync(entity);
                if (res <= 0)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new InvalidOperationException("Failed to update test history");
                }

                await _unitOfWork.CommitTransactionAsync();
                return entity.ToResponse();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> RemoveAsync(int id)
        {
            if (id <= 0) return false;
            var entity = await _unitOfWork.TestHistoryRepository.GetByIdAsync(id);
            if (entity == null) return false;

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var ok = await _unitOfWork.TestHistoryRepository.RemoveAsync(entity);
                if (!ok)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return false;
                }

                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
