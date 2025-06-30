using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObjects.DTO;
using BusinessObjects.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repositories.IRepository;
using Services.IService;

namespace Services.Service
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AccountService(IAccountRepository accountRepository, IMapper mapper, IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<SystemAccountDTO> LoginAsync(LoginDTO loginDTO)
        {
            var account = await _accountRepository.GetAccountByEmailAsync(loginDTO.Email);
            if (account == null || !BCrypt.Net.BCrypt.Verify(loginDTO.Password, account.AccountPassword))
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            // Tạo JWT token
            var token = GenerateJwtToken(account);

            // Sử dụng AutoMapper để ánh xạ từ SystemAccount sang SystemAccountDTO
            var accountDTO = _mapper.Map<SystemAccountDTO>(account);
            accountDTO.Token = token;  // Thêm token vào DTO

            return accountDTO;
        }

        private string GenerateJwtToken(SystemAccount account)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
                new Claim(ClaimTypes.Name, account.AccountEmail),
                new Claim(ClaimTypes.Role, account.AccountRole)  // Lưu role vào token
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])); // Lấy khóa bí mật từ appsettings.json
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],  // Cấu hình Issuer trong appsettings.json
                audience: _configuration["Jwt:Audience"],  // Cấu hình Audience trong appsettings.json
                claims: claims,
                expires: DateTime.Now.AddHours(1), // Token hết hạn sau 1 giờ
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);  // Trả về token dưới dạng chuỗi
        }

        public async Task<SystemAccountDTO?> GetAccountByEmailAsync(string email)
        {
            var account = await _accountRepository.GetAccountByEmailAsync(email);
            if (account == null) return null;
            return _mapper.Map<SystemAccountDTO>(account);
        }

        public async Task CreateAccountAsync(CreateAccountDTO createAccountDTO)
        {
            var existingAccountByEmail = await _accountRepository.GetAccountByEmailAsync(createAccountDTO.AccountEmail);
            if (existingAccountByEmail != null)
            {
                throw new InvalidOperationException("Email already exists.");
            }

            var account = _mapper.Map<SystemAccount>(createAccountDTO);

            if (string.IsNullOrWhiteSpace(createAccountDTO.AccountPassword))
            {
                throw new ArgumentException("Password cannot be empty.", nameof(createAccountDTO.AccountPassword));
            }
            account.AccountPassword = BCrypt.Net.BCrypt.HashPassword(createAccountDTO.AccountPassword);

            await _accountRepository.CreateAccountAsync(account);
        }

        public async Task<SystemAccountDTO?> GetAccountByIdAsync(int id)
        {
            var account = await _accountRepository.GetAccountByIdAsync(id); // Giả sử GetAccountByIdAsync trả về SystemAccount
            if (account == null)
            {
                return null;
            }
            return _mapper.Map<SystemAccountDTO>(account);
        }
        public async Task UpdateAccountAsync(int id, CreateAccountDTO createAccountDTO) 
        {
            var account = await _accountRepository.GetAccountByIdAsync(id);
            if (account == null)
            {
                throw new KeyNotFoundException("Account not found.");
            }

            if (account.AccountEmail != createAccountDTO.AccountEmail)
            {
                var existingAccountByEmail = await _accountRepository.GetAccountByEmailAsync(createAccountDTO.AccountEmail);
                if (existingAccountByEmail != null && existingAccountByEmail.AccountId != id)
                {
                    throw new InvalidOperationException("New email already used by another account.");
                }
            }

            account.AccountName = createAccountDTO.AccountName;
            account.AccountEmail = createAccountDTO.AccountEmail;
            account.AccountRole = createAccountDTO.AccountRole;

            if (!string.IsNullOrWhiteSpace(createAccountDTO.AccountPassword))
            {
                account.AccountPassword = BCrypt.Net.BCrypt.HashPassword(createAccountDTO.AccountPassword);
            }

            await _accountRepository.UpdateAccountAsync(account);
        }

        public async Task DeleteAccountAsync(int id)
        {
            var account = await _accountRepository.GetAccountByIdAsync(id);
            if (account == null)
            {
                throw new KeyNotFoundException("Account not found.");
            }

            if (account.CreatedNewsArticles.Any())
            {
                throw new InvalidOperationException("Cannot delete account as it has created news articles.");
            }

            await _accountRepository.DeleteAccountAsync(id);
        }

        public async Task<IEnumerable<SystemAccountDTO>> SearchAccountsAsync(string? searchQuery)
        {
            var accounts = await _accountRepository.SearchAccountsAsync(searchQuery ?? string.Empty);
            return _mapper.Map<IEnumerable<SystemAccountDTO>>(accounts);
        }

        public async Task<SystemAccountDTO?> GetProfileAsync(int userId)
        {
            var account = await _accountRepository.GetAccountByIdAsync(userId);
            if (account == null)
            {
                throw new KeyNotFoundException("User profile not found.");
            }
            return _mapper.Map<SystemAccountDTO>(account);
        }

        public async Task<SystemAccountDTO?> UpdateProfileAsync(int userId, UpdateProfileDto updateProfileDto)
        {
            var account = await _accountRepository.GetAccountByIdAsync(userId);
            if (account == null)
            {
                throw new KeyNotFoundException("User profile not found.");
            }

            // Kiểm tra nếu email mới (nếu thay đổi) đã được người khác sử dụng
            if (account.AccountEmail != updateProfileDto.AccountEmail)
            {
                var existingAccount = await _accountRepository.GetAccountByEmailAsync(updateProfileDto.AccountEmail);
                if (existingAccount != null && existingAccount.AccountId != userId)
                {
                    throw new ArgumentException("Email is already in use by another account.");
                }
            }

            account.AccountName = updateProfileDto.AccountName;
            account.AccountEmail = updateProfileDto.AccountEmail;

            await _accountRepository.UpdateAccountAsync(account);
            return _mapper.Map<SystemAccountDTO>(account);
        }

        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto)
        {
            var account = await _accountRepository.GetAccountByIdAsync(userId);
            if (account == null)
            {
                throw new KeyNotFoundException("User profile not found.");
            }

            // 1. Xác thực mật khẩu hiện tại
            if (!BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, account.AccountPassword))
            {
                throw new ArgumentException("Incorrect current password.");
            }

            // 2. Hash mật khẩu mới và cập nhật
            account.AccountPassword = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);

            await _accountRepository.UpdateAccountAsync(account);
            return true;
        }
    }

}
