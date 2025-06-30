using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Configs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using BCryptNet = BCrypt.Net.BCrypt;
using BusinessObjects.Models;

namespace DataAccessObjects
{
    public static class DataSeeder
    {
        public static async Task InitializeAdminUserAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FUNewsManagementSystemDbContext>();
                var adminSettings = scope.ServiceProvider.GetRequiredService<IOptions<AdminAccountSettings>>().Value;

                if (!await context.SystemAccounts.AnyAsync(u => u.AccountEmail == adminSettings.Email))
                {
                    var adminUser = new SystemAccount
                    {
                        AccountName = adminSettings.AccountName ?? "Administrator",
                        AccountEmail = adminSettings.Email,
                        AccountPassword = BCryptNet.HashPassword(adminSettings.Password), 
                        AccountRole = adminSettings.Role ?? "Admin"
                    };

                    await context.SystemAccounts.AddAsync(adminUser);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}