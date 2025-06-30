using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.DTO;

namespace Services.IService
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryViewDto>> GetAllActiveCategoriesAsync();

        Task<CategoryViewDto?> GetCategoryByIdAsync(int categoryId);

        Task<CategoryViewDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto);

        Task<CategoryViewDto?> UpdateCategoryAsync(int categoryId, UpdateCategoryDto updateCategoryDto);

        Task<bool> DeleteCategoryAsync(int categoryId);
    }
}
