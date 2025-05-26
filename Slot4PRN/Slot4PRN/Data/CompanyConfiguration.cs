using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Slot4PRN.Models;

namespace Slot4PRN.Data
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            // map sang bảng
            builder.ToTable("Companies");

            // khóa chính
            builder.HasKey(c => c.Id);

            // các property
            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(60);
            builder.Property(c => c.Address)
                   .IsRequired()
                   .HasMaxLength(60);
            builder.Property(c => c.Country)
                   .HasMaxLength(50);

            // quan hệ 1-n với Employee
            builder.HasMany(c => c.Employees)
                   .WithOne(e => e.Company)
                   .HasForeignKey(e => e.CompanyId);

            // seed dữ liệu mẫu
            builder.HasData(
                new Company
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "Acme Corporation",
                    Address = "123 Elm Street",
                    Country = "USA"
                },
                new Company
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "Globex International",
                    Address = "456 Oak Avenue",
                    Country = "Canada"
                }
            );
        }
    }
}
