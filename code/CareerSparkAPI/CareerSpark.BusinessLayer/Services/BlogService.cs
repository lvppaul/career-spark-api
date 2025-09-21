using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.DTOs.Response;
using CareerSpark.BusinessLayer.DTOs.Update;
using CareerSpark.BusinessLayer.Interfaces;
using CareerSpark.BusinessLayer.Mappings;
using CareerSpark.DataAccessLayer.Helper;
using CareerSpark.DataAccessLayer.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CareerSpark.BusinessLayer.Services
{
    public class BlogService : IBlogService
    {
        private readonly IUnitOfWork _unitOfWork;
        public BlogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<BlogResponse>> GetAllAsync()
        {
            var blogs = await _unitOfWork.BlogRepository.GetAllAsync();
            if (blogs == null || !blogs.Any())
                return Enumerable.Empty<BlogResponse>();
            return blogs.Select(BlogMapper.ToResponse).ToList();
        }

        public async Task<PaginatedResult<BlogResponse>> GetAllAsyncWithPagination(Pagination pagination)
        {
            var result = await _unitOfWork.BlogRepository.GetAllAsyncWithPagination(pagination);

            if (result.Items == null || !result.Items.Any())
            {
                return new PaginatedResult<BlogResponse>(
                    Enumerable.Empty<BlogResponse>(),
                    0,
                    pagination.PageNumber,
                    pagination.PageSize
                );
            }

            var blogResponses = result.Items.Select(BlogMapper.ToResponse).ToList();

            return new PaginatedResult<BlogResponse>(
                blogResponses,
                result.TotalCount,
                result.PageNumber,
                result.PageSize
            );
        }

        public async Task<BlogResponse> GetByIdAsync(int id)
        {
            var blog = await _unitOfWork.BlogRepository.GetByIdAsync(id);
            return blog?.ToResponse() ?? new BlogResponse();
        }

        public async Task<BlogResponse> UpdateAsync(int id, BlogUpdate blogUpdate)
        {
            // blog truyền vào null hoặc Id <= 0
            if (blogUpdate == null)
                throw new ArgumentNullException(nameof(blogUpdate), "Blog update data cannot be null");

            if (id <= 0)
                throw new ArgumentException("Invalid blog ID", nameof(id));

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // Get existing blog
                var existingBlog = await _unitOfWork.BlogRepository.GetByIdAsync(id);
                if (existingBlog == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new InvalidOperationException($"Blog with ID {id} not found");
                }

                // Map về lại Entity
                BlogMapper.ToUpdate(blogUpdate, existingBlog);

                var updateResult = await _unitOfWork.BlogRepository.UpdateAsync(existingBlog);

                if (updateResult == 0)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new InvalidOperationException("Failed to update blog information - no changes were saved");
                }

                await _unitOfWork.CommitTransactionAsync();

                // Map về Response
                return BlogMapper.ToResponse(existingBlog);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<BlogResponse> CreateAsync(BlogRequest blogRequest)
        {
            // blog truyền vào null
            if (blogRequest == null)
                throw new ArgumentNullException(nameof(blogRequest), "Blog request data cannot be null");

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // Map Request về Entity
                var blogEntity = BlogMapper.ToEntity(blogRequest);

                var createResult = await _unitOfWork.BlogRepository.CreateAsync(blogEntity);

                if (createResult == 0)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new InvalidOperationException("Failed to create blog - no changes were saved");
                }

                await _unitOfWork.CommitTransactionAsync();

                // Map về Response
                return BlogMapper.ToResponse(blogEntity);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid blog ID", nameof(id));

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // Get existing blog
                var existingBlog = await _unitOfWork.BlogRepository.GetByIdAsync(id);
                if (existingBlog == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return false;
                }

                var deleteResult = await _unitOfWork.BlogRepository.RemoveAsync(existingBlog);

                if (!deleteResult)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return false;
                }

                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<IEnumerable<BlogResponse>> SearchByTitleAsync(string title)
        {
            var blogs = await _unitOfWork.BlogRepository.GetBlogsByTitleAsync(title);
            if (blogs == null || !blogs.Any())
                return Enumerable.Empty<BlogResponse>();
            return blogs.Select(BlogMapper.ToResponse).ToList();
        }

        public async Task<IEnumerable<BlogResponse>> GetBlogsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var blogs = await _unitOfWork.BlogRepository.GetBlogsByDateRangeAsync(startDate, endDate);
            if (blogs == null || !blogs.Any())
                return Enumerable.Empty<BlogResponse>();
            return blogs.Select(BlogMapper.ToResponse).ToList();
        }

        public async Task<IEnumerable<BlogResponse>> GetRecentBlogsAsync(int count = 10)
        {
            var blogs = await _unitOfWork.BlogRepository.GetRecentBlogsAsync(count);
            if (blogs == null || !blogs.Any())
                return Enumerable.Empty<BlogResponse>();
            return blogs.Select(BlogMapper.ToResponse).ToList();
        }

        public async Task<IEnumerable<BlogResponse>> GetPublishedBlogsAsync()
        {
            var blogs = await _unitOfWork.BlogRepository.GetPublishedBlogsAsync();
            if (blogs == null || !blogs.Any())
                return Enumerable.Empty<BlogResponse>();
            return blogs.Select(BlogMapper.ToResponse).ToList();
        }

        public async Task<bool> UpdateBlogPublishedAsync(int id)
        {
            var blog = await _unitOfWork.BlogRepository.GetByIdAsync(id);
            if (blog == null) return false;

            blog.IsPublished = true;
            _unitOfWork.BlogRepository.PrepareUpdate(blog);

            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> UpdateBlogUnpublishedAsync(int id)
        {
            var blog = await _unitOfWork.BlogRepository.GetByIdAsync(id);
            if (blog == null) return false;

            blog.IsPublished = false;
            _unitOfWork.BlogRepository.PrepareUpdate(blog);

            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> UpdateBlogDeletedAsync(int id)
        {
            var blog = await _unitOfWork.BlogRepository.GetByIdAsync(id);
            if (blog == null) return false;

            blog.IsDeleted = true;
            _unitOfWork.BlogRepository.PrepareUpdate(blog);

            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
