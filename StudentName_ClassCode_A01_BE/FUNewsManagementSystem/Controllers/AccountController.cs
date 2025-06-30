using BusinessObjects.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.IService;
namespace FUNewsManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                var result = await _accountService.LoginAsync(loginDTO);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDTO createAccountDTO)
        {
            try
            {
                await _accountService.CreateAccountAsync(createAccountDTO);
                return Ok("Account created successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> GetAccounts([FromQuery] string? searchQuery)
        {
            try
            {
                var accounts = await _accountService.SearchAccountsAsync(searchQuery);
                if (accounts == null || !accounts.Any())
                {
                    return NotFound("No accounts found.");
                }
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> GetAccountById(int id) 
        {
            try
            {
                var accountDTO = await _accountService.GetAccountByIdAsync(id);
                if (accountDTO == null)
                {
                    return NotFound(new { message = "Account not found." });
                }
                var accountToReturn = new SystemAccountDTO
                {
                    AccountId = accountDTO.AccountId,
                    AccountName = accountDTO.AccountName,
                    AccountEmail = accountDTO.AccountEmail,
                    AccountRole = accountDTO.AccountRole
                };
                return Ok(accountToReturn);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAccount(int id, [FromBody] CreateAccountDTO createAccountDTO)
        {
            try
            {
                await _accountService.UpdateAccountAsync(id, createAccountDTO);
                return Ok("Account updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            try
            {
                await _accountService.DeleteAccountAsync(id);
                return Ok("Account deleted successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("logout")]
        [Authorize] 
        public IActionResult Logout()
        {
            return Ok(new { message = "Logout successful. Please clear the token on the client side." });
        }
    }


}

