using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.DTOs.Response;
using CareerSpark.BusinessLayer.DTOs.Update;
using CareerSpark.BusinessLayer.Interfaces;
using CareerSpark.BusinessLayer.Mappings;
using CareerSpark.DataAccessLayer.Entities;
using CareerSpark.DataAccessLayer.Helper;
using CareerSpark.DataAccessLayer.UnitOfWork;
using Microsoft.EntityFrameworkCore;
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

        public async Task<SubmitTestResponse> SubmitAsync(SubmitTestRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (request.UserId <= 0) throw new ArgumentException("Invalid user id", nameof(request.UserId));
            if (request.Answers == null || request.Answers.Count == 0) throw new ArgumentException("Answers are required", nameof(request.Answers));

            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);
            if (user == null) throw new InvalidOperationException($"User with id {request.UserId} not found");



            await _unitOfWork.BeginTransactionAsync();
            try
            {
                // create TestAnswers from user answers
                var createdAnswers = new List<TestAnswer>();

                foreach (var ans in request.Answers)
                {
                    var qa = await _unitOfWork.QuestionTestRepository.GetByIdAsync(ans.QuestionId);
                    if (qa == null)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        throw new InvalidOperationException($"Question with id {ans.QuestionId} not found");
                    }

                    var answer = new TestAnswer
                    {
                        Content = ans.IsSelected ? "Có" : "Không",
                        IsSelected = ans.IsSelected,
                        QuestionId = ans.QuestionId
                    };
                    var res = await _unitOfWork.TestAnswerRepository.CreateAsync(answer);
                    if (res <= 0)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        throw new InvalidOperationException("Failed to create test answer");
                    }
                    createdAnswers.Add(answer);
                }

                // create TestHistory with ResultId = 0 (temporary)
                foreach (var ans in createdAnswers)
                {
                    var history = new DataAccessLayer.Entities.TestHistory
                    {
                        UserId = request.UserId,
                        TestAnswerId = ans.Id,
                        ResultId = 0
                    };
                    var res = await _unitOfWork.TestHistoryRepository.CreateAsync(history);
                    if (res <= 0)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        throw new InvalidOperationException("Failed to create test history");
                    }
                }

               
            
           

            // Calculate scores by joining TestAnswer -> QuestionTest and filtering by histories of user
            var allAnswers = await _unitOfWork.TestAnswerRepository.GetAllAsync();
            var allQuestions = await _unitOfWork.QuestionTestRepository.GetAllAsync();
            var allHistories = await _unitOfWork.TestHistoryRepository.GetAllAsync();
            var userAnswerIds = allHistories.Where(h => h.UserId == request.UserId).Select(h => h.TestAnswerId).ToHashSet();

            var joined = from a in allAnswers
                         join q in allQuestions on a.QuestionId equals q.Id
                         where userAnswerIds.Contains(a.Id)
                         group a by q.QuestionType into g
                         select new { Type = g.Key, Score = g.Count(x => x.IsSelected == true) };

            int R = joined.FirstOrDefault(s => s.Type == "Realistic")?.Score ?? 0;
            int I = joined.FirstOrDefault(s => s.Type == "Investigative")?.Score ?? 0;
            int A = joined.FirstOrDefault(s => s.Type == "Artistic")?.Score ?? 0;
            int S = joined.FirstOrDefault(s => s.Type == "Social")?.Score ?? 0;
            int E = joined.FirstOrDefault(s => s.Type == "Enterprising")?.Score ?? 0;
            int C = joined.FirstOrDefault(s => s.Type == "Conventional")?.Score ?? 0;

            // Save Result and update histories
            var resultEntity = new DataAccessLayer.Entities.Result
            {
                Content = $"Kết quả RIASEC cho User {request.UserId}",
                R = R,
                I = I,
                A = A,
                S = S,
                E = E,
                C = C
            };

         
                var created = await _unitOfWork.ResultRepository.CreateAsync(resultEntity);
                if (created <= 0)
                {
                    throw new InvalidOperationException("Failed to create result");
                }

                var historiesToUpdate = allHistories.Where(h => h.UserId == request.UserId && h.ResultId == 0).ToList();
                foreach (var h in historiesToUpdate)
                {
                    h.ResultId = resultEntity.Id;
                }
                var updated = await _unitOfWork.TestHistoryRepository.UpdateRangeAsync(historiesToUpdate);
                if (updated <= 0)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new InvalidOperationException("Failed to update histories with result id");
                }

                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            return new SubmitTestResponse
            {
                ResultId = resultEntity.Id,
                R = R,
                I = I,
                A = A,
                S = S,
                E = E,
                C = C
            };
        }
    }
}
