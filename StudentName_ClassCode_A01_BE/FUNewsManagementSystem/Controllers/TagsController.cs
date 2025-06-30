using BusinessObjects.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.IService;

namespace FUNewsManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;
        private readonly ILogger<TagsController> _logger;

        public TagsController(ITagService tagService, ILogger<TagsController> logger)
        {
            _tagService = tagService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTags()
        {
            try
            {
                var tags = await _tagService.GetAllTagsAsync();
                if (tags == null || !tags.Any())
                {
                    return Ok(new List<TagViewDto>()); 
                }
                return Ok(tags);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all tags.");
                return StatusCode(500, new { message = "An error occurred while fetching tags. Please try again later." });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTagById(int id)
        {
            try
            {
                var tag = await _tagService.GetTagByIdAsync(id);
                if (tag == null)
                {
                    return NotFound(new { message = $"Tag with ID {id} not found." });
                }
                return Ok(tag);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching tag with ID {id}.");
                return StatusCode(500, new { message = "An error occurred while fetching the tag. Please try again later." });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> CreateTag([FromBody] CreateTagDto createTagDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdTag = await _tagService.CreateTagAsync(createTagDto);
                return CreatedAtAction(nameof(GetTagById), new { id = createdTag.TagId }, createdTag);
            }
            catch (ArgumentException ex) 
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new tag.");
                return StatusCode(500, new { message = "An error occurred while creating the tag. Please try again later." });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> UpdateTag(int id, [FromBody] UpdateTagDto updateTagDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedTag = await _tagService.UpdateTagAsync(id, updateTagDto);
                if (updatedTag == null)
                {
                    return NotFound(new { message = $"Tag with ID {id} not found." });
                }
                return Ok(updatedTag);
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
                _logger.LogError(ex, $"Error occurred while updating tag with ID {id}.");
                return StatusCode(500, new { message = "An error occurred while updating the tag. Please try again later." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            try
            {
                var success = await _tagService.DeleteTagAsync(id);
                if (success)
                {
                    return Ok(new { message = $"Tag with ID {id} deleted successfully." }); 
                }
                return NotFound(new { message = $"Tag with ID {id} not found or could not be deleted (it might be in use)." });
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
                _logger.LogError(ex, $"Error occurred while deleting tag with ID {id}.");
                return StatusCode(500, new { message = "An error occurred while deleting the tag. Please try again later." });
            }
        }
    }
}
