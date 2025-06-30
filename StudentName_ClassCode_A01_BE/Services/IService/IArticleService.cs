using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.DTO;

namespace Services.IService
{
    public interface IArticleService
    {
        Task<IEnumerable<ArticleViewDto>> GetAllArticlesAsync();
        Task<ArticleViewDto?> GetArticleByIdAsync(int articleId);
        Task<ArticleViewDto?> CreateArticleForStaffAsync(StaffCreateArticleDto createArticleDto, int staffUserId);
        Task<ArticleViewDto?> UpdateArticleForStaffAsync(int articleId, StaffUpdateArticleDto updateArticleDto, int staffUserId);
        Task<bool> DeleteArticleForStaffAsync(int articleId, int staffUserId);
        Task<IEnumerable<ArticleViewDto>> GetArticlesByStaffAsync(int staffUserId); 
        Task<ArticleViewDto?> GetArticleDetailsForStaffAsync(int articleId, int staffUserId);
        Task<IEnumerable<ArticleViewDto>> GenerateArticleReportAsync(DateTime startDate, DateTime endDate, string? sortOrder);

    }
}
