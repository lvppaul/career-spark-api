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
    public interface IQuestionTestService
    {
        Task<IEnumerable<QuestionTestResponse>> GetAllAsync();
        Task<PaginatedResult<QuestionTestResponse>> GetAllAsyncWithPagination(Pagination pagination);
        Task<QuestionTestResponse?> GetByIdAsync(int id);
        Task<QuestionTestResponse> CreateAsync(QuestionTestRequest request);
        Task<QuestionTestResponse> UpdateAsync(int id, QuestionTestUpdate update);
        Task<bool> RemoveAsync(int id);
        Task<SubmitTestResponse> SubmitAsync(SubmitTestRequest request);
    }
}
