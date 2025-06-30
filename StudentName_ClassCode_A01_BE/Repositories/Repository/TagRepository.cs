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
    public class TagRepository : ITagRepository
    {
        private readonly FUNewsManagementSystemDbContext _context;

        public TagRepository(FUNewsManagementSystemDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        {
            return await _context.Tags.OrderBy(t => t.TagName).ToListAsync();
        }

        public async Task<Tag?> GetTagByIdAsync(int tagId)
        {
            return await _context.Tags.FindAsync(tagId);
        }

        public async Task CreateTagAsync(Tag tag)
        {
            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTagAsync(Tag tag)
        {
            _context.Tags.Update(tag);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTagAsync(int tagId)
        {
            var tag = await _context.Tags.FindAsync(tagId);
            if (tag != null)
            {
                _context.Tags.Remove(tag);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> TagExistsAsync(int tagId)
        {
            return await _context.Tags.AnyAsync(t => t.TagId == tagId);
        }

        public async Task<bool> TagNameExistsAsync(string tagName, int? currentTagId = null)
        {
            if (currentTagId.HasValue)
            {
                return await _context.Tags.AnyAsync(t => t.TagName == tagName && t.TagId != currentTagId.Value);
            }
            return await _context.Tags.AnyAsync(t => t.TagName == tagName);
        }

        public async Task<bool> IsTagAssignedToArticlesAsync(int tagId)
        {
            return await _context.NewsTags.AnyAsync(nt => nt.TagId == tagId);
        }
    }
}
