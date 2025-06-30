using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using DataAccessObjects;
using Microsoft.EntityFrameworkCore;
using Repositories.IRepository;

namespace Repositories.Repository
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly FUNewsManagementSystemDbContext _context;

        public ArticleRepository(FUNewsManagementSystemDbContext context)
        {
            _context = context;
        }

        private IQueryable<NewsArticle> IncludeArticleDetails(IQueryable<NewsArticle> query)
        {
            return query.Include(a => a.Category)
                        .Include(a => a.CreatedBy)
                        .Include(a => a.UpdatedBy)
                        .Include(a => a.NewsTags)
                            .ThenInclude(nt => nt.Tag);
        }

        public async Task<IEnumerable<NewsArticle>> GetAllArticlesAsync(bool includeDetails = false)
        {
            IQueryable<NewsArticle> query = _context.NewsArticles.AsQueryable();
            if (includeDetails)
            {
                query = IncludeArticleDetails(query);
            }
            return await query.OrderByDescending(a => a.CreatedDate).ToListAsync();
        }

        public async Task<NewsArticle?> GetArticleByIdAsync(int articleId, bool includeDetails = false)
        {
            IQueryable<NewsArticle> query = _context.NewsArticles.Where(a => a.NewsArticleId == articleId);
            if (includeDetails)
            {
                query = IncludeArticleDetails(query);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<NewsArticle>> GetArticlesByAuthorIdAsync(int authorId, bool includeDetails = false)
        {
            IQueryable<NewsArticle> query = _context.NewsArticles.Where(a => a.CreatedById == authorId);
            if (includeDetails)
            {
                query = IncludeArticleDetails(query);
            }
            return await query.OrderByDescending(a => a.CreatedDate).ToListAsync();
        }

        public async Task<IEnumerable<NewsArticle>> GetArticlesByCategoryIdAsync(int categoryId, bool includeDetails = false)
        {
            IQueryable<NewsArticle> query = _context.NewsArticles.Where(a => a.CategoryId == categoryId);
            if (includeDetails)
            {
                query = IncludeArticleDetails(query);
            }
            return await query.OrderByDescending(a => a.CreatedDate).ToListAsync();
        }

        public async Task CreateArticleAsync(NewsArticle article)
        {
            await _context.NewsArticles.AddAsync(article);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateArticleAsync(NewsArticle article)
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteArticleAsync(int articleId)
        {
            var article = await _context.NewsArticles.Include(a => a.NewsTags).FirstOrDefaultAsync(a => a.NewsArticleId == articleId);
            if (article != null)
            {
                _context.NewsArticles.Remove(article);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ArticleExistsAsync(int articleId)
        {
            return await _context.NewsArticles.AnyAsync(a => a.NewsArticleId == articleId);
        }
        public async Task<IEnumerable<NewsArticle>> GetArticlesByDateRangeAsync(DateTime startDate, DateTime endDate, string? sortOrder, bool includeDetails = false)
        {
            var inclusiveEndDate = endDate.Date.AddDays(1).AddTicks(-1);
            IQueryable<NewsArticle> query = _context.NewsArticles
                .Where(a => a.CreatedDate >= startDate.Date && a.CreatedDate <= inclusiveEndDate);

            if (string.Equals(sortOrder, "asc", StringComparison.OrdinalIgnoreCase))
            {
                query = query.OrderBy(a => a.CreatedDate); 
            }
            else
            {
                query = query.OrderByDescending(a => a.CreatedDate); 
            }

            if (includeDetails)
            {
                query = IncludeArticleDetails(query);
            }

            return await query.ToListAsync();
        }
    }
}
