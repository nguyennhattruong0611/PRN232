using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using DataAccessObjects;
using Microsoft.EntityFrameworkCore;
using Repositories.IRepository;

namespace Repositories.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly FUNewsManagementSystemDbContext _context;

        public AccountRepository(FUNewsManagementSystemDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<SystemAccount>> GetAllAccountsAsync()
        {
            return await _context.SystemAccounts.AsNoTracking().ToListAsync(); 
        }


        public async Task<SystemAccount> GetAccountByEmailAsync(string email)
        {
            return await _context.SystemAccounts.SingleOrDefaultAsync(a => a.AccountEmail == email);
        }

        public async Task<SystemAccount?> GetAccountByIdAsync(int id) 
        {
            return await _context.SystemAccounts
                                 .Include(a => a.CreatedNewsArticles) 
                                 .FirstOrDefaultAsync(a => a.AccountId == id);
        }

        public async Task<IEnumerable<SystemAccount>> SearchAccountsAsync(string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return await GetAllAccountsAsync(); 
            }
            return await _context.SystemAccounts
                .Where(a => a.AccountEmail.Contains(searchQuery) || a.AccountName.Contains(searchQuery))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task CreateAccountAsync(SystemAccount account)
        {
            await _context.SystemAccounts.AddAsync(account);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAccountAsync(SystemAccount account)
        {
            _context.SystemAccounts.Update(account);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAccountAsync(int id)
        {
            var account = await GetAccountByIdAsync(id);
            if (account != null)
            {
                _context.SystemAccounts.Remove(account);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> AccountExistsAsync(int id)
        {
            return await _context.SystemAccounts.AnyAsync(e => e.AccountId == id);
        }
    }

}
