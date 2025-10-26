using CareerSpark.DataAccessLayer.Context;
using CareerSpark.DataAccessLayer.Entities;
using CareerSpark.DataAccessLayer.Helper;
using CareerSpark.DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerSpark.DataAccessLayer.Repositories
{
    public class NewsRepository : GenericRepository<News>, INewsRepository
    {
        public NewsRepository(CareerSparkDbContext context) : base(context)
        {
        }

        public override async Task<News> GetByIdAsync(int id)
        {
            return await _context.News
                .Where(b => b.IsActive)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public override async Task<List<News>> GetAllAsync()
        {
            return await _context.News
                .Where(b => b.IsActive)
                .OrderByDescending(b => b.CreatedAt) // Order by newest first
                .ToListAsync();
        }

        public override async Task<PaginatedResult<News>> GetAllAsyncWithPagination(Pagination pagination)
        {
            var query = _context.News
                        .Where(n => n.IsActive)
                        .OrderByDescending(n => n.CreatedAt)
                        .AsQueryable();

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return new PaginatedResult<News>(items, totalCount, pagination.PageNumber, pagination.PageSize);
        }
    }
}
