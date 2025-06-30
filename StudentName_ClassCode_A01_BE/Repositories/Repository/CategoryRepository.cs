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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly FUNewsManagementSystemDbContext _context;

        public CategoryRepository(FUNewsManagementSystemDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(bool includeChildren = false)
        {
            IQueryable<Category> query = _context.Categories.Where(c => c.IsActive == true);

            if (includeChildren)
            {
                query = query.Include(c => c.ChildCategories);
            }
            return await query.OrderBy(c => c.CategoryName).ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int categoryId, bool includeArticles = false, bool includeChildren = false)
        {
            IQueryable<Category> query = _context.Categories.Where(c => c.CategoryId == categoryId && c.IsActive == true);

            if (includeArticles)
            {
                query = query.Include(c => c.NewsArticles);
            }
            if (includeChildren)
            {
                query = query.Include(c => c.ChildCategories);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task CreateCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> CategoryExistsAsync(int categoryId)
        {
            return await _context.Categories.AnyAsync(c => c.CategoryId == categoryId && c.IsActive == true);
        }

        public async Task<bool> CategoryHasArticlesAsync(int categoryId)
        {
            return await _context.NewsArticles.AnyAsync(n => n.CategoryId == categoryId);
        }

        public async Task<bool> CategoryNameExistsAsync(string categoryName, int? currentCategoryId = null)
        {
            if (currentCategoryId.HasValue)
            {
                return await _context.Categories.AnyAsync(c => c.CategoryName == categoryName && c.CategoryId != currentCategoryId.Value && c.IsActive == true);
            }
            return await _context.Categories.AnyAsync(c => c.CategoryName == categoryName && c.IsActive == true);
        }
    }
}
