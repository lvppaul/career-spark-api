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
    public interface IResultService 
    {
        Task<IEnumerable<ResultResponse>> GetAllAsync();
        Task<PaginatedResult<ResultResponse>> GetAllAsyncWithPagination(Pagination pagination);
        Task<ResultResponse?> GetByIdAsync(int id);
        Task<ResultResponse> CreateAsync(ResultRequest request);
        Task<ResultResponse> UpdateAsync(int id, ResultUpdate update);
        Task<bool> RemoveAsync(int id);
    }
}
