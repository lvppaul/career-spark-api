using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerSpark.BusinessLayer.Interfaces
{
    public interface ITestService
    {
        Task<StartTestResponse> StartTestAsync(StartTestRequest request);
        Task<TestResultResponse> SubmitTestAsync(SubmitTestRequest request);
        Task<CareerPathResponse> GetRoadmapAsync(int sessionId, int userId);
        Task<TestHistoryResponse> GetTestHistoryAsync(int sessionId, int userId);
    }
}
