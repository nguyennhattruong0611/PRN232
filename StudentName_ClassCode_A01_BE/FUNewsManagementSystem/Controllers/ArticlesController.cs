using BusinessObjects.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.IService;
using System.Security.Claims;

namespace FUNewsManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly ILogger<ArticlesController> _logger;

        public ArticlesController(IArticleService articleService, ILogger<ArticlesController> logger)
        {
            _articleService = articleService;
            _logger = logger;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            throw new UnauthorizedAccessException("User ID not found in token.");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllArticles()
        {
            try
            {
                var articles = await _articleService.GetAllArticlesAsync();
                if (articles == null || !articles.Any())
                {
                    return Ok(new List<ArticleViewDto>());
                }
                return Ok(articles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching articles.", details = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> CreateArticle([FromBody] StaffCreateArticleDto createArticleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var staffUserId = GetCurrentUserId();
                var articleView = await _articleService.CreateArticleForStaffAsync(createArticleDto, staffUserId);
                if (articleView == null)
                {
                    return BadRequest(new { message = "Failed to create article. Please check input data (e.g., valid CategoryId, TagIds)." });
                }
                return CreatedAtAction(nameof(GetArticleById), new { id = articleView.NewsArticleId }, articleView);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the article.", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> UpdateArticle(int id, [FromBody] StaffUpdateArticleDto updateArticleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var staffUserId = GetCurrentUserId();
                var articleView = await _articleService.UpdateArticleForStaffAsync(id, updateArticleDto, staffUserId);
                return Ok(articleView);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                // <<< SỬA LỖI Ở ĐÂY
                return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the article.", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            try
            {
                var staffUserId = GetCurrentUserId();
                await _articleService.DeleteArticleForStaffAsync(id, staffUserId);
                return Ok(new { message = "Article deleted successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                // <<< SỬA LỖI Ở ĐÂY
                return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Log lỗi
                return StatusCode(500, new { message = "An error occurred while deleting the article.", details = ex.Message });
            }
        }

        [HttpGet("my-articles")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> GetMyArticles()
        {
            try
            {
                var staffUserId = GetCurrentUserId();
                var articles = await _articleService.GetArticlesByStaffAsync(staffUserId);
                if (articles == null || !articles.Any())
                {
                    return Ok(new List<ArticleViewDto>());
                }
                return Ok(articles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching your articles.", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetArticleById(int id)
        {
            try
            {
                var articleDto = await _articleService.GetArticleByIdAsync(id);

                if (articleDto == null)
                {
                    return NotFound(new { message = $"Article with ID {id} not found." });
                }

                if (articleDto.NewsStatus == true)
                {
                    return Ok(articleDto);
                }

                if (User.Identity != null && User.Identity.IsAuthenticated)
                {
                    if (User.IsInRole("Staff"))
                    {
                        return Ok(articleDto);
                    }

                    var currentUserId = GetCurrentUserId();
                    if (currentUserId > 0 && articleDto.CreatedById == currentUserId)
                    {
                        return Ok(articleDto);
                    }
                }
                return NotFound(new { message = $"Article with ID {id} not found." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching article with ID {id}.");
                return StatusCode(500, new { message = "An internal error occurred." });
            }
        }
    }
}