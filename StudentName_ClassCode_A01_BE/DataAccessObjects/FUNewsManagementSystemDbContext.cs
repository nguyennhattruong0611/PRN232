using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObjects
{
    public class FUNewsManagementSystemDbContext : DbContext
    {
        public FUNewsManagementSystemDbContext(DbContextOptions<FUNewsManagementSystemDbContext> options)
            : base(options)
        {
        }

        public DbSet<SystemAccount> SystemAccounts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<NewsArticle> NewsArticles { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<NewsTag> NewsTags { get; set; }

        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasOne(c => c.ParentCategory)
                      .WithMany(c => c.ChildCategories)
                      .HasForeignKey(c => c.ParentCategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<NewsArticle>(entity =>
            {
                entity.HasOne(n => n.Category)
                      .WithMany(c => c.NewsArticles)
                      .HasForeignKey(n => n.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(n => n.CreatedBy)
                      .WithMany(a => a.CreatedNewsArticles)
                      .HasForeignKey(n => n.CreatedById)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(n => n.UpdatedBy)
                      .WithMany(a => a.UpdatedNewsArticles)
                      .HasForeignKey(n => n.UpdatedById)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<NewsTag>(entity =>
            {
                entity.HasKey(nt => new { nt.NewsArticleId, nt.TagId });

                entity.HasOne(nt => nt.NewsArticle)
                      .WithMany(n => n.NewsTags)
                      .HasForeignKey(nt => nt.NewsArticleId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(nt => nt.Tag)
                      .WithMany(t => t.NewsTags)
                      .HasForeignKey(nt => nt.TagId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // modelBuilder.Entity<SystemAccount>().HasData(
            // new SystemAccount
            // {
            // AccountId = 1,
            // AccountName = "Administrator",
            // AccountEmail = "admin@FUNewsManagementSystem.org",
            // AccountPassword = "@@abc123@@",
            // AccountRole = "Admin"
            // }
            // );
        }
    }



}