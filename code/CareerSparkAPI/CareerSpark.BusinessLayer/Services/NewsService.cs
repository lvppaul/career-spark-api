using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.DTOs.Response;
using CareerSpark.BusinessLayer.DTOs.Update;
using CareerSpark.BusinessLayer.Interfaces;
using CareerSpark.BusinessLayer.Mappings;
using CareerSpark.DataAccessLayer.Entities;
using CareerSpark.DataAccessLayer.Helper;
using CareerSpark.DataAccessLayer.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerSpark.BusinessLayer.Services
{
    public class NewsService : INewsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICloudinaryService _cloudinaryService;

        public NewsService(IUnitOfWork unitOfWork, ICloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
            _unitOfWork = unitOfWork;
        }

        public async Task<NewsResponse> CreateNewsAsync(NewsRequest news)
        {
            if(news == null) throw new ArgumentNullException(nameof(news));
            var newsEntity = NewsMapper.ToEntity(news);

            if(news.ImageFile != null && news.ImageFile.Length > 0)
            {
                var validation = _cloudinaryService.ValidateDocumentFile(news.ImageFile);
                if (!validation.IsValid)
                    throw new ArgumentException(validation.ErrorMessage);

                var uploadResult = await _cloudinaryService.UploadFileAsync(news.ImageFile, "NewsImage");
                newsEntity.ImageUrl = uploadResult.SecureUrl.ToString();
                newsEntity.avatarPublicId = uploadResult.PublicId;
            }

            await _unitOfWork.NewsRepository.CreateAsync(newsEntity);
            await _unitOfWork.SaveAsync();

            var newsResponse = NewsMapper.ToResponse(newsEntity);

            return newsResponse;
        }

        public async Task<bool> Deactive(int id)
        {
            var news = await _unitOfWork.NewsRepository.GetByIdAsync(id);

            if(news == null)
                return false;

            news.IsActive = false;
            await _unitOfWork.NewsRepository.UpdateAsync(news);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task<bool> DeleteNewsAsync(int id)
        {
            var news = await _unitOfWork.NewsRepository.GetByIdAsync(id);
            
            if(news == null)
                return false;

            news.IsActive = false;
            await _unitOfWork.NewsRepository.UpdateAsync(news);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task<IEnumerable<NewsResponse>> GetAllActiveNewsAsync()
        {
            var news = await _unitOfWork.NewsRepository.GetAllAsync();

            if (news == null || !news.Any())
                return Enumerable.Empty<NewsResponse>();
            return news.Select(NewsMapper.ToResponse).ToList();
        }

        public async Task<NewsResponse?> GetNewsByIdAsync(int id)
        {
            var news = await _unitOfWork.NewsRepository.GetByIdAsync(id);
            return NewsMapper.ToResponse(news) ?? new NewsResponse();
        }

        public async Task<NewsResponse?> UpdateNewsAsync(int id, NewsRequest updatedNews)
        {
            if (updatedNews == null)
                throw new ArgumentNullException(nameof(updatedNews), "News update data cannot be null");

            if (id <= 0)
                throw new ArgumentException("Invalid news ID", nameof(id));

            try
            {
                var existingNews = await _unitOfWork.NewsRepository.GetByIdAsync(id);
                if (existingNews == null)
                {
                    return null;
                }

                existingNews.Title = updatedNews.Title?.Trim() ?? existingNews.Title;
                existingNews.Content = updatedNews.Content?.Trim() ?? existingNews.Content;

                if (updatedNews.ImageFile != null && updatedNews.ImageFile.Length > 0)
                {
                    // Validate file
                    var validation = _cloudinaryService.ValidateDocumentFile(updatedNews.ImageFile);
                    if (!validation.IsValid)
                        throw new ArgumentException(validation.ErrorMessage);

                    if (!string.IsNullOrEmpty(existingNews.avatarPublicId))
                    {
                        await _cloudinaryService.DeleteFileAsync(existingNews.avatarPublicId);
                    }

                    var uploadResult = await _cloudinaryService.UploadFileAsync(updatedNews.ImageFile, "NewsImages");

                    existingNews.ImageUrl = uploadResult.SecureUrl;
                    existingNews.avatarPublicId = uploadResult.PublicId;
                }

                await _unitOfWork.NewsRepository.UpdateAsync(existingNews);
                await _unitOfWork.SaveAsync();

                return NewsMapper.ToResponse(existingNews);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the news item: {ex.Message}", ex);
            }
        }
    }
}
