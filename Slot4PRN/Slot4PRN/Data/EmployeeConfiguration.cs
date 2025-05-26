using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Slot4PRN.Models;

namespace Slot4PRN.Data
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                   .IsRequired()
                   .HasMaxLength(30);
            builder.Property(e => e.Age)
                   .IsRequired();
            builder.Property(e => e.Position)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.HasOne(e => e.Company)
                   .WithMany(c => c.Employees)
                   .HasForeignKey(e => e.CompanyId);

            builder.HasData(
                new Employee
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    Name = "John Doe",
                    Age = 30,
                    Position = "Manager",
                    CompanyId = Guid.Parse("11111111-1111-1111-1111-111111111111")
                },
                new Employee
                {
                    Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    Name = "Jane Smith",
                    Age = 25,
                    Position = "Developer",
                    CompanyId = Guid.Parse("11111111-1111-1111-1111-111111111111")
                },
                new Employee
                {
                    Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    Name = "Bob Johnson",
                    Age = 28,
                    Position = "Tester",
                    CompanyId = Guid.Parse("22222222-2222-2222-2222-222222222222")
                }
            );
        }
    }
}