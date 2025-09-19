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
    public class ResultService : IResultService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ResultService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ResultResponse>> GetAllAsync()
        {
            var items = await _unitOfWork.ResultRepository.GetAllAsync();
            if (items == null || !items.Any()) return Enumerable.Empty<ResultResponse>();
            return items.Select(ResultMapper.ToResponse).ToList();
        }

        public async Task<PaginatedResult<ResultResponse>> GetAllAsyncWithPagination(Pagination pagination)
        {
            var result = await _unitOfWork.ResultRepository.GetAllAsyncWithPagination(pagination);
            if (result.Items == null || !result.Items.Any())
            {
                return new PaginatedResult<ResultResponse>(Enumerable.Empty<ResultResponse>(), 0, pagination.PageNumber, pagination.PageSize);
            }

            var mapped = result.Items.Select(ResultMapper.ToResponse).ToList();
            return new PaginatedResult<ResultResponse>(mapped, result.TotalCount, result.PageNumber, result.PageSize);
        }

        public async Task<ResultResponse?> GetByIdAsync(int id)
        {
            if (id <= 0) return null;
            var entity = await _unitOfWork.ResultRepository.GetByIdAsync(id);
            return entity?.ToResponse();
        }

        public async Task<ResultResponse> CreateAsync(ResultRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var entity = request.ToEntity();

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var res = await _unitOfWork.ResultRepository.CreateAsync(entity);
                if (res <= 0)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new InvalidOperationException("Failed to create result");
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

        public async Task<ResultResponse> UpdateAsync(int id, ResultUpdate update)
        {
            if (id <= 0) throw new ArgumentException("Invalid id", nameof(id));
            if (update == null) throw new ArgumentNullException(nameof(update));

            var entity = await _unitOfWork.ResultRepository.GetByIdAsync(id);
            if (entity == null) throw new InvalidOperationException($"Result with id {id} not found");

            update.ToUpdate(entity);

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var res = await _unitOfWork.ResultRepository.UpdateAsync(entity);
                if (res <= 0)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new InvalidOperationException("Failed to update result");
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
            var entity = await _unitOfWork.ResultRepository.GetByIdAsync(id);
            if (entity == null) return false;

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var ok = await _unitOfWork.ResultRepository.RemoveAsync(entity);
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
