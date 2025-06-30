using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.DTO;

namespace Services.IService
{
    public interface IAccountService
    {
        Task<SystemAccountDTO> LoginAsync(LoginDTO loginDTO);
        Task CreateAccountAsync(CreateAccountDTO createAccountDTO);
        Task<SystemAccountDTO?> GetAccountByIdAsync(int id);
        Task<SystemAccountDTO?> GetAccountByEmailAsync(string email); 

        Task UpdateAccountAsync(int id, CreateAccountDTO createAccountDTO);
        Task DeleteAccountAsync(int id);
        Task<IEnumerable<SystemAccountDTO>> SearchAccountsAsync(string? searchQuery);

        Task<SystemAccountDTO?> GetProfileAsync(int userId);
        Task<SystemAccountDTO?> UpdateProfileAsync(int userId, UpdateProfileDto updateProfileDto);
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);

    }

}
