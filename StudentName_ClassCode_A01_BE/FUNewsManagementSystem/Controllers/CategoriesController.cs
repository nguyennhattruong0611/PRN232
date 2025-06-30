using BusinessObjects.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.IService;

namespace FUNewsManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoriesController> _logger; 

        public CategoriesController(ICategoryService categoryService, ILogger<CategoriesController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllActiveCategories()
        {
            try
            {
                var categories = await _categoryService.GetAllActiveCategoriesAsync();
                if (categories == null || !categories.Any())
                {
                    return Ok(new List<CategoryViewDto>()); 
                }
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all active categories.");
                return StatusCode(500, new { message = "An error occurred while fetching categories. Please try again later." });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);
                if (category == null)
                {
                    return NotFound(new { message = $"Category with ID {id} not found." });
                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching category with ID {id}.");
                return StatusCode(500, new { message = "An error occurred while fetching the category. Please try again later." });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdCategory = await _categoryService.CreateCategoryAsync(createCategoryDto);
                return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.CategoryId }, createdCategory);
            }
            catch (ArgumentException ex) 
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new category.");
                return StatusCode(500, new { message = "An error occurred while creating the category. Please try again later." });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto updateCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedCategory = await _categoryService.UpdateCategoryAsync(id, updateCategoryDto);
                if (updatedCategory == null) 
                {
                    return NotFound(new { message = $"Category with ID {id} not found." });
                }
                return Ok(updatedCategory);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex) 
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating category with ID {id}.");
                return StatusCode(500, new { message = "An error occurred while updating the category. Please try again later." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var success = await _categoryService.DeleteCategoryAsync(id);
                if (success)
                {
                    return Ok(new { message = $"Category with ID {id} deleted successfully." }); 
                }
                return NotFound(new { message = $"Category with ID {id} not found or could not be deleted." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting category with ID {id}.");
                return StatusCode(500, new { message = "An error occurred while deleting the category. Please try again later." });
            }
        }
    }
}
