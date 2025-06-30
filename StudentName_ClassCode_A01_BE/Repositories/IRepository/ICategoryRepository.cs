using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace Repositories.IRepository
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync(bool includeChildren = false);
        Task<Category?> GetCategoryByIdAsync(int categoryId, bool includeArticles = false, bool includeChildren = false);
        Task CreateCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int categoryId);
        Task<bool> CategoryExistsAsync(int categoryId);
        Task<bool> CategoryHasArticlesAsync(int categoryId); // Kiểm tra xem category có bài viết nào không
        Task<bool> CategoryNameExistsAsync(string categoryName, int? currentCategoryId = null); // Để kiểm tra tên category trùng
    }
}
