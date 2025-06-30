using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace Repositories.IRepository
{
    public interface IArticleRepository
    {
        Task<IEnumerable<NewsArticle>> GetAllArticlesAsync(bool includeDetails = false);
        Task<NewsArticle?> GetArticleByIdAsync(int articleId, bool includeDetails = false);
        Task<IEnumerable<NewsArticle>> GetArticlesByAuthorIdAsync(int authorId, bool includeDetails = false);
        Task<IEnumerable<NewsArticle>> GetArticlesByCategoryIdAsync(int categoryId, bool includeDetails = false);
        Task CreateArticleAsync(NewsArticle article);
        Task UpdateArticleAsync(NewsArticle article); 
        Task DeleteArticleAsync(int articleId);
        Task<bool> ArticleExistsAsync(int articleId);
        Task<IEnumerable<NewsArticle>> GetArticlesByDateRangeAsync(DateTime startDate, DateTime endDate, string? sortOrder, bool includeDetails = false);

    }
}
