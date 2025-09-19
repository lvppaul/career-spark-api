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
    public class TestAnswerService : ITestAnswerService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TestAnswerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TestAnswerResponse>> GetAllAsync()
        {
            var items = await _unitOfWork.TestAnswerRepository.GetAllAsync();
            if (items == null || !items.Any()) return Enumerable.Empty<TestAnswerResponse>();
            return items.Select(TestAnswerMapper.ToResponse).ToList();
        }

        public async Task<PaginatedResult<TestAnswerResponse>> GetAllAsyncWithPagination(Pagination pagination)
        {
            var result = await _unitOfWork.TestAnswerRepository.GetAllAsyncWithPagination(pagination);
            if (result.Items == null || !result.Items.Any())
            {
                return new PaginatedResult<TestAnswerResponse>(Enumerable.Empty<TestAnswerResponse>(), 0, pagination.PageNumber, pagination.PageSize);
            }

            var mapped = result.Items.Select(TestAnswerMapper.ToResponse).ToList();
            return new PaginatedResult<TestAnswerResponse>(mapped, result.TotalCount, result.PageNumber, result.PageSize);
        }

        public async Task<TestAnswerResponse?> GetByIdAsync(int id)
        {
            if (id <= 0) return null;
            var entity = await _unitOfWork.TestAnswerRepository.GetByIdAsync(id);
            return entity?.ToResponse();
        }

        public async Task<IEnumerable<TestAnswerResponse>> GetByQuestionIdAsync(int questionId)
        {
            if (questionId <= 0) return Enumerable.Empty<TestAnswerResponse>();
            var all = await _unitOfWork.TestAnswerRepository.GetAllAsync();
            var filtered = all.Where(x => x.QuestionId == questionId);
            return filtered.Select(TestAnswerMapper.ToResponse).ToList();
        }

        public async Task<TestAnswerResponse> CreateAsync(TestAnswerRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrWhiteSpace(request.Content)) throw new ArgumentException("Content is required", nameof(request.Content));
            if (request.QuestionId <= 0) throw new ArgumentException("QuestionId is invalid", nameof(request.QuestionId));

            // ensure question exists
            var question = await _unitOfWork.QuestionTestRepository.GetByIdAsync(request.QuestionId);
            if (question == null) throw new InvalidOperationException($"Question with id {request.QuestionId} not found");

            var entity = request.ToEntity();

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var res = await _unitOfWork.TestAnswerRepository.CreateAsync(entity);
                if (res <= 0)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new InvalidOperationException("Failed to create test answer");
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

        public async Task<TestAnswerResponse> UpdateAsync(int id, TestAnswerUpdate update)
        {
            if (id <= 0) throw new ArgumentException("Invalid id", nameof(id));
            if (update == null) throw new ArgumentNullException(nameof(update));

            var entity = await _unitOfWork.TestAnswerRepository.GetByIdAsync(id);
            if (entity == null) throw new InvalidOperationException($"TestAnswer with id {id} not found");

            update.ToUpdate(entity);

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var res = await _unitOfWork.TestAnswerRepository.UpdateAsync(entity);
                if (res <= 0)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new InvalidOperationException("Failed to update test answer");
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
            var entity = await _unitOfWork.TestAnswerRepository.GetByIdAsync(id);
            if (entity == null) return false;

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var ok = await _unitOfWork.TestAnswerRepository.RemoveAsync(entity);
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
