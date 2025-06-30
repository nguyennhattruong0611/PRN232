using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObjects.DTO;
using BusinessObjects.Models;
using Repositories.IRepository;
using Services.IService;

namespace Services.Service
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public TagService(ITagRepository tagRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TagViewDto>> GetAllTagsAsync()
        {
            var tags = await _tagRepository.GetAllTagsAsync();
            return _mapper.Map<IEnumerable<TagViewDto>>(tags);
        }

        public async Task<TagViewDto?> GetTagByIdAsync(int tagId)
        {
            var tag = await _tagRepository.GetTagByIdAsync(tagId);
            if (tag == null)
            {
                return null;
            }
            return _mapper.Map<TagViewDto>(tag);
        }

        public async Task<TagViewDto?> CreateTagAsync(CreateTagDto createTagDto)
        {
            if (await _tagRepository.TagNameExistsAsync(createTagDto.TagName))
            {
                throw new ArgumentException($"Tag with name '{createTagDto.TagName}' already exists.");
            }

            var tag = _mapper.Map<Tag>(createTagDto);
            await _tagRepository.CreateTagAsync(tag);
            return _mapper.Map<TagViewDto>(tag);
        }

        public async Task<TagViewDto?> UpdateTagAsync(int tagId, UpdateTagDto updateTagDto)
        {
            var existingTag = await _tagRepository.GetTagByIdAsync(tagId);
            if (existingTag == null)
            {
                throw new KeyNotFoundException($"Tag with ID {tagId} not found.");
            }

            if (existingTag.TagName != updateTagDto.TagName && await _tagRepository.TagNameExistsAsync(updateTagDto.TagName, tagId))
            {
                throw new ArgumentException($"Tag with name '{updateTagDto.TagName}' already exists.");
            }

            _mapper.Map(updateTagDto, existingTag);
            await _tagRepository.UpdateTagAsync(existingTag);

            return _mapper.Map<TagViewDto>(existingTag);
        }

        public async Task<bool> DeleteTagAsync(int tagId)
        {
            var tag = await _tagRepository.GetTagByIdAsync(tagId);
            if (tag == null)
            {
                throw new KeyNotFoundException($"Tag with ID {tagId} not found.");
            }

            if (await _tagRepository.IsTagAssignedToArticlesAsync(tagId))
            {
                throw new InvalidOperationException($"Tag '{tag.TagName}' cannot be deleted as it is currently assigned to one or more articles.");
            }

            await _tagRepository.DeleteTagAsync(tagId);
            return true;
        }
    }
}
