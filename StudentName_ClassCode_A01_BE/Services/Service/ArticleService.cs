using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObjects.DTO;
using BusinessObjects.Models;
using DataAccessObjects;
using Microsoft.EntityFrameworkCore;
using Repositories.IRepository;
using Services.IService;

namespace Services.Service
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;
        private readonly FUNewsManagementSystemDbContext _context;

        public ArticleService(
            IArticleRepository articleRepository,
            ICategoryRepository categoryRepository,
            ITagRepository tagRepository,
            IMapper mapper,
            FUNewsManagementSystemDbContext context)
        {
            _articleRepository = articleRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
            _mapper = mapper;
            _context = context;
        }
        public async Task<IEnumerable<ArticleViewDto>> GenerateArticleReportAsync(DateTime startDate, DateTime endDate, string? sortOrder)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException("Start date cannot be after end date.");
            }

            var articles = await _articleRepository.GetArticlesByDateRangeAsync(startDate, endDate, sortOrder, includeDetails: true);

            return _mapper.Map<IEnumerable<ArticleViewDto>>(articles);
        }
        public async Task<IEnumerable<ArticleViewDto>> GetAllArticlesAsync()
        {
            var articles = await _articleRepository.GetAllArticlesAsync(includeDetails: true);
            return _mapper.Map<IEnumerable<ArticleViewDto>>(articles);
        }
        public async Task<ArticleViewDto?> GetArticleByIdAsync(int articleId)
        {
            var article = await _articleRepository.GetArticleByIdAsync(articleId, includeDetails: true);

            if (article == null)
            {
                return null;
            }

            return _mapper.Map<ArticleViewDto>(article);
        }
        public async Task<ArticleViewDto?> CreateArticleForStaffAsync(StaffCreateArticleDto createArticleDto, int staffUserId)
        {
            if (!await _categoryRepository.CategoryExistsAsync(createArticleDto.CategoryId))
            {
                throw new ArgumentException($"Category with ID {createArticleDto.CategoryId} does not exist.");
            }

            var article = _mapper.Map<NewsArticle>(createArticleDto);
            article.CreatedById = staffUserId;
            article.CreatedDate = DateTime.UtcNow;
            article.NewsStatus = false;

            if (createArticleDto.TagIds != null && createArticleDto.TagIds.Any())
            {
                foreach (var tagId in createArticleDto.TagIds.Distinct())
                {
                    if (!await _tagRepository.TagExistsAsync(tagId))
                    {
                        _context.Logs.Add(new Log { Message = $"Attempted to add non-existent TagId {tagId} to new article by User {staffUserId}", LoggedDate = DateTime.UtcNow, Level = "Warning" });
                        continue;
                    }
                    article.NewsTags.Add(new NewsTag { TagId = tagId });
                }
            }

            await _articleRepository.CreateArticleAsync(article);

            var createdArticleWithDetails = await _articleRepository.GetArticleByIdAsync(article.NewsArticleId, includeDetails: true);
            return _mapper.Map<ArticleViewDto>(createdArticleWithDetails);
        }

        public async Task<ArticleViewDto?> UpdateArticleForStaffAsync(int articleId, StaffUpdateArticleDto updateArticleDto, int staffUserId)
        {
            var article = await _context.NewsArticles
                                        .Include(a => a.NewsTags)
                                        .FirstOrDefaultAsync(a => a.NewsArticleId == articleId);

            if (article == null)
            {
                throw new KeyNotFoundException($"Article with ID {articleId} not found.");
            }

            // <<< KHỐI LỆNH KIỂM TRA QUYỀN SỞ HỮU ĐÃ ĐƯỢC XÓA BỎ TẠI ĐÂY >>>
            // if (article.CreatedById != staffUserId)
            // {
            //     throw new UnauthorizedAccessException("You are not authorized to update this article.");
            // }

            if (!await _categoryRepository.CategoryExistsAsync(updateArticleDto.CategoryId))
            {
                throw new ArgumentException($"Category with ID {updateArticleDto.CategoryId} does not exist.");
            }

            _mapper.Map(updateArticleDto, article);
            article.ModifiedDate = DateTime.UtcNow;
            article.UpdatedById = staffUserId;

            if (updateArticleDto.NewsStatus.HasValue)
            {
                article.NewsStatus = updateArticleDto.NewsStatus.Value;
            }

            var currentTagIds = article.NewsTags.Select(nt => nt.TagId).ToList();
            var newTagIds = updateArticleDto.TagIds?.Distinct().ToList() ?? new List<int>();

            var tagsToRemove = article.NewsTags.Where(nt => !newTagIds.Contains(nt.TagId)).ToList();
            if (tagsToRemove.Any())
            {
                _context.NewsTags.RemoveRange(tagsToRemove);
            }

            foreach (var tagIdToAdd in newTagIds)
            {
                if (!currentTagIds.Contains(tagIdToAdd))
                {
                    if (!await _tagRepository.TagExistsAsync(tagIdToAdd))
                    {
                        _context.Logs.Add(new Log { Message = $"Attempted to add non-existent TagId {tagIdToAdd} to article {articleId} by User {staffUserId}", LoggedDate = DateTime.UtcNow, Level = "Warning" });
                        continue;
                    }
                    article.NewsTags.Add(new NewsTag { NewsArticleId = articleId, TagId = tagIdToAdd });
                }
            }

            await _context.SaveChangesAsync();

            var updatedArticleWithDetails = await _articleRepository.GetArticleByIdAsync(articleId, includeDetails: true);
            return _mapper.Map<ArticleViewDto>(updatedArticleWithDetails);
        }

        public async Task<bool> DeleteArticleForStaffAsync(int articleId, int staffUserId)
        {
            var article = await _articleRepository.GetArticleByIdAsync(articleId, includeDetails: false);

            if (article == null)
            {
                throw new KeyNotFoundException($"Article with ID {articleId} not found.");
            }

            // <<< KHỐI LỆNH KIỂM TRA QUYỀN SỞ HỮU ĐÃ ĐƯỢC XÓA BỎ TẠI ĐÂY >>>
            // if (article.CreatedById != staffUserId)
            // {
            //     throw new UnauthorizedAccessException("You are not authorized to delete this article.");
            // }

            if (article.NewsStatus == true)
            {
                throw new InvalidOperationException("Cannot delete an article that has been approved. Please contact an administrator.");
            }

            await _articleRepository.DeleteArticleAsync(articleId);
            return true;
        }

        public async Task<IEnumerable<ArticleViewDto>> GetArticlesByStaffAsync(int staffUserId)
        {
            var articles = await _articleRepository.GetArticlesByAuthorIdAsync(staffUserId, includeDetails: true);
            return _mapper.Map<IEnumerable<ArticleViewDto>>(articles);
        }

        public async Task<ArticleViewDto?> GetArticleDetailsForStaffAsync(int articleId, int staffUserId)
        {
            var article = await _articleRepository.GetArticleByIdAsync(articleId, includeDetails: true);

            if (article == null)
            {
                return null;
            }

            if (article.CreatedById != staffUserId)
            {
                return null;
            }

            return _mapper.Map<ArticleViewDto>(article);
        }
    }
}