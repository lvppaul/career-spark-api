using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.DTOs.Response;
using CareerSpark.BusinessLayer.DTOs.Update;
using CareerSpark.BusinessLayer.Interfaces;
using CareerSpark.BusinessLayer.Mappings;
using CareerSpark.DataAccessLayer.Helper;
using CareerSpark.DataAccessLayer.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CareerSpark.BusinessLayer.Services
{
    public class QuestionTestService : IQuestionTestService
    {
        private readonly IUnitOfWork _unitOfWork;
        public QuestionTestService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<QuestionTestResponse>> GetAllAsync()
        {
            var items = await _unitOfWork.QuestionTestRepository.GetAllAsync();
            if (items == null || !items.Any()) return Enumerable.Empty<QuestionTestResponse>();
            return items.Select(QuestionTestMapper.ToResponse).ToList();
        }

        public async Task<PaginatedResult<QuestionTestResponse>> GetAllAsyncWithPagination(Pagination pagination)
        {
            var result = await _unitOfWork.QuestionTestRepository.GetAllAsyncWithPagination(pagination);
            if (result.Items == null || !result.Items.Any())
            {
                return new PaginatedResult<QuestionTestResponse>(Enumerable.Empty<QuestionTestResponse>(), 0, pagination.PageNumber, pagination.PageSize);
            }

            var mapped = result.Items.Select(QuestionTestMapper.ToResponse).ToList();
            return new PaginatedResult<QuestionTestResponse>(mapped, result.TotalCount, result.PageNumber, result.PageSize);
        }

        public async Task<QuestionTestResponse?> GetByIdAsync(int id)
        {
            if (id <= 0) return null;
            var entity = await _unitOfWork.QuestionTestRepository.GetByIdAsync(id);
            return entity?.ToResponse();
        }

        public async Task<QuestionTestResponse> CreateAsync(QuestionTestRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrWhiteSpace(request.Content)) throw new ArgumentException("Content is required", nameof(request.Content));

            var entity = request.ToEntity();

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var res = await _unitOfWork.QuestionTestRepository.CreateAsync(entity);
                if (res <= 0)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new InvalidOperationException("Failed to create question test");
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

        public async Task<QuestionTestResponse> UpdateAsync(int id, QuestionTestUpdate update)
        {
            if (id <= 0) throw new ArgumentException("Invalid id", nameof(id));
            if (update == null) throw new ArgumentNullException(nameof(update));

            var entity = await _unitOfWork.QuestionTestRepository.GetByIdAsync(id);
            if (entity == null) throw new InvalidOperationException($"QuestionTest with id {id} not found");

            update.ToUpdate(entity);

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var res = await _unitOfWork.QuestionTestRepository.UpdateAsync(entity);
                if (res <= 0)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new InvalidOperationException("Failed to update question test");
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
            var entity = await _unitOfWork.QuestionTestRepository.GetByIdAsync(id);
            if (entity == null) return false;

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var ok = await _unitOfWork.QuestionTestRepository.RemoveAsync(entity);
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
