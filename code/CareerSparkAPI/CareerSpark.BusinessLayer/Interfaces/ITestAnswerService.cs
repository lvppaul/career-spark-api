using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.DTOs.Response;
using CareerSpark.BusinessLayer.DTOs.Update;
using CareerSpark.DataAccessLayer.Helper;

namespace CareerSpark.BusinessLayer.Interfaces
{
    public interface ITestAnswerService
    {
        Task<IEnumerable<TestAnswerResponse>> GetAllAsync();
        Task<PaginatedResult<TestAnswerResponse>> GetAllAsyncWithPagination(Pagination pagination);
        Task<TestAnswerResponse?> GetByIdAsync(int id);
        Task<IEnumerable<TestAnswerResponse>> GetByQuestionIdAsync(int questionId);
        Task<TestAnswerResponse> CreateAsync(TestAnswerRequest request);
        Task<TestAnswerResponse> UpdateAsync(int id, TestAnswerUpdate update);
        Task<bool> RemoveAsync(int id);
    }
}
