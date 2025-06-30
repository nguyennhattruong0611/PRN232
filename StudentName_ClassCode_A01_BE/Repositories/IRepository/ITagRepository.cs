using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace Repositories.IRepository
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllTagsAsync();
        Task<Tag?> GetTagByIdAsync(int tagId);
        Task CreateTagAsync(Tag tag);
        Task UpdateTagAsync(Tag tag);
        Task DeleteTagAsync(int tagId);
        Task<bool> TagExistsAsync(int tagId);
        Task<bool> TagNameExistsAsync(string tagName, int? currentTagId = null);
        Task<bool> IsTagAssignedToArticlesAsync(int tagId); 
    }
}
