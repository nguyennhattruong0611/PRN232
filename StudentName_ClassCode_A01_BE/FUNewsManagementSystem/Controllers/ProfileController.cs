using BusinessObjects.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.IService;
using System.Security.Claims;

namespace FUNewsManagementSystem.Controllers
{
    [Route("api/profile")]
    [ApiController]
    [Authorize] // Yêu cầu tất cả các action trong đây phải được xác thực
    public class ProfileController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(IAccountService accountService, ILogger<ProfileController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            throw new UnauthorizedAccessException("User ID not found in token.");
        }

        // GET: api/profile/me
        // Lấy thông tin cá nhân của người dùng đang đăng nhập
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            try
            {
                var userId = GetCurrentUserId();
                var profile = await _accountService.GetProfileAsync(userId);
                // Không trả về token khi xem profile
                profile.Token = null;
                return Ok(profile);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user profile.");
                return StatusCode(500, new { message = "An error occurred." });
            }
        }

        // PUT: api/profile/me
        // Cập nhật thông tin cá nhân của người dùng đang đăng nhập
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateProfileDto updateProfileDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var userId = GetCurrentUserId();
                var updatedProfile = await _accountService.UpdateProfileAsync(userId, updateProfileDto);
                updatedProfile.Token = null;
                return Ok(updatedProfile);
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
                _logger.LogError(ex, "Error occurred while updating user profile.");
                return StatusCode(500, new { message = "An error occurred." });
            }
        }

        // POST: api/profile/change-password
        // Đổi mật khẩu của người dùng đang đăng nhập
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangeMyPassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var userId = GetCurrentUserId();
                var success = await _accountService.ChangePasswordAsync(userId, changePasswordDto);
                if (success)
                {
                    return Ok(new { message = "Password changed successfully." });
                }
                return BadRequest(new { message = "Failed to change password." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex) // Bắt lỗi mật khẩu cũ không đúng
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while changing password.");
                return StatusCode(500, new { message = "An error occurred." });
            }
        }
    }
}
