using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObjects.DTO;
using BusinessObjects.Models;
using Repositories.IRepository;
using Services.IService;

namespace Services.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryViewDto>> GetAllActiveCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            return _mapper.Map<IEnumerable<CategoryViewDto>>(categories);
        }

        public async Task<CategoryViewDto?> GetCategoryByIdAsync(int categoryId)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
            if (category == null)
            {
                return null;
            }
            return _mapper.Map<CategoryViewDto>(category);
        }

        public async Task<CategoryViewDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            if (await _categoryRepository.CategoryNameExistsAsync(createCategoryDto.CategoryName))
            {
                throw new ArgumentException($"Category with name '{createCategoryDto.CategoryName}' already exists.");
            }

            if (createCategoryDto.ParentCategoryId.HasValue)
            {
                if (!await _categoryRepository.CategoryExistsAsync(createCategoryDto.ParentCategoryId.Value))
                {
                    throw new ArgumentException($"Parent category with ID '{createCategoryDto.ParentCategoryId.Value}' does not exist.");
                }
            }

            var category = _mapper.Map<Category>(createCategoryDto);

            await _categoryRepository.CreateCategoryAsync(category);
            return _mapper.Map<CategoryViewDto>(category);
        }

        public async Task<CategoryViewDto?> UpdateCategoryAsync(int categoryId, UpdateCategoryDto updateCategoryDto)
        {
            var existingCategory = await _categoryRepository.GetCategoryByIdAsync(categoryId);
            if (existingCategory == null)
            {
                throw new KeyNotFoundException($"Category with ID {categoryId} not found.");
            }

            if (existingCategory.CategoryName != updateCategoryDto.CategoryName &&
                await _categoryRepository.CategoryNameExistsAsync(updateCategoryDto.CategoryName, categoryId))
            {
                throw new ArgumentException($"Category with name '{updateCategoryDto.CategoryName}' already exists.");
            }

            if (updateCategoryDto.ParentCategoryId.HasValue)
            {
                if (updateCategoryDto.ParentCategoryId.Value == categoryId) 
                {
                    throw new ArgumentException("A category cannot be its own parent.");
                }
                if (!await _categoryRepository.CategoryExistsAsync(updateCategoryDto.ParentCategoryId.Value))
                {
                    throw new ArgumentException($"Parent category with ID '{updateCategoryDto.ParentCategoryId.Value}' does not exist.");
                }
            }
            else
            {
                // Nếu ParentCategoryId được set thành null (trở thành category gốc)
                // không cần kiểm tra gì thêm về parent.
            }


            _mapper.Map(updateCategoryDto, existingCategory);
            await _categoryRepository.UpdateCategoryAsync(existingCategory);

            return _mapper.Map<CategoryViewDto>(existingCategory);
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId, includeArticles: true, includeChildren: true);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID {categoryId} not found.");
            }

            if (await _categoryRepository.CategoryHasArticlesAsync(categoryId))
            {
                throw new InvalidOperationException($"Category '{category.CategoryName}' cannot be deleted as it is currently associated with one or more articles.");
            }

            if (category.ChildCategories != null && category.ChildCategories.Any(c => c.IsActive)) 
            {
                throw new InvalidOperationException($"Category '{category.CategoryName}' cannot be deleted as it has active child categories. Please delete or reassign child categories first.");
            }

            await _categoryRepository.DeleteCategoryAsync(categoryId);
            return true;
        }
    }
}
