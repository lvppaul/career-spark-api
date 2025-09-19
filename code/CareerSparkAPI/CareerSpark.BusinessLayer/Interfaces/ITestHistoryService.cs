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
    public interface ITestHistoryService
    {
        Task<IEnumerable<TestHistoryResponse>> GetAllAsync();
        Task<PaginatedResult<TestHistoryResponse>> GetAllAsyncWithPagination(Pagination pagination);
        Task<TestHistoryResponse?> GetByIdAsync(int id);
        Task<IEnumerable<TestHistoryResponse>> GetByUserIdAsync(int userId);
        Task<TestHistoryResponse> CreateAsync(TestHistoryRequest request);
        Task<TestHistoryResponse> UpdateAsync(int id, TestHistoryUpdate update);
        Task<bool> RemoveAsync(int id);
    }
}
