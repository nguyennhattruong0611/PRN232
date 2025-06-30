using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.IService;

namespace FUNewsManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] 
    public class ReportsController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly ILogger<ReportsController> _logger;

        public ReportsController(IArticleService articleService, ILogger<ReportsController> logger)
        {
            _articleService = articleService;
            _logger = logger;
        }

        [HttpGet("articles")]
        public async Task<IActionResult> GetArticlesReport(
       [FromQuery] DateTime startDate,
       [FromQuery] DateTime endDate,
       [FromQuery] string? sortOrder = "desc") 
        {
            try
            {
                var reportData = await _articleService.GenerateArticleReportAsync(startDate, endDate, sortOrder);

                if (reportData == null || !reportData.Any())
                {
                    return Ok(new List<object>());
                }
                return Ok(reportData);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating the articles report.");
                return StatusCode(500, new { message = "An internal error occurred while generating the report." });
            }
        }
    }
}
