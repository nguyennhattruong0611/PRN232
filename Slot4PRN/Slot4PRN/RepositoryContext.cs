using Microsoft.EntityFrameworkCore;
using Slot4PRN.Data;
using Slot4PRN.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Slot4PRN
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options)
     : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                     "Data Source=DESKTOP-EECT8AK;Database=Slot5PRN;User Id=sa;Password=nhattruong123;TrustServerCertificate=true;Trusted_Connection=SSPI;Encrypt=false;");
            }
        }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
