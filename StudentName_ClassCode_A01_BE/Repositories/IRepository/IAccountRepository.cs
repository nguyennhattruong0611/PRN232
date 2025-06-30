using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace Repositories.IRepository
{
    public interface IAccountRepository
    {
        Task<SystemAccount> GetAccountByEmailAsync(string email);
        Task<SystemAccount> GetAccountByIdAsync(int id);
        Task<IEnumerable<SystemAccount>> SearchAccountsAsync(string searchQuery);
        Task CreateAccountAsync(SystemAccount account);
        Task UpdateAccountAsync(SystemAccount account);
        Task DeleteAccountAsync(int id);
        Task<bool> AccountExistsAsync(int id);
        Task<IEnumerable<SystemAccount>> GetAllAccountsAsync(); 

    }

}
