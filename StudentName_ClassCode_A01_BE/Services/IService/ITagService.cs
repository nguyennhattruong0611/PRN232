using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.DTO;

namespace Services.IService
{
    public interface ITagService
    {
        Task<IEnumerable<TagViewDto>> GetAllTagsAsync();
        Task<TagViewDto?> GetTagByIdAsync(int tagId);
        Task<TagViewDto?> CreateTagAsync(CreateTagDto createTagDto); 
        Task<TagViewDto?> UpdateTagAsync(int tagId, UpdateTagDto updateTagDto); 
        Task<bool> DeleteTagAsync(int tagId); 
    }
}
