using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.DTOs.Response;
using CareerSpark.DataAccessLayer.Entities;
using CareerSpark.DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerSpark.BusinessLayer.Interfaces
{
    public interface INewsService
    {
        Task<IEnumerable<NewsResponse>> GetAllActiveNewsAsync();
        Task<NewsResponse?> GetNewsByIdAsync(int id);
        Task<NewsResponse> CreateNewsAsync(NewsRequest news);
        Task<NewsResponse?> UpdateNewsAsync(int id, NewsRequest news);
        Task<bool> DeleteNewsAsync(int id);
        Task<bool> Deactive(int id);
    }
}
