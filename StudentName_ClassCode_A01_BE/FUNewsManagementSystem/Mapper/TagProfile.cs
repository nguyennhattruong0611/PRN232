using AutoMapper;
using BusinessObjects.DTO;
using BusinessObjects.Models;

namespace FUNewsManagementSystem.Mapper
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<Tag, TagViewDto>();
            CreateMap<CreateTagDto, Tag>();
            CreateMap<UpdateTagDto, Tag>();
        }
    }
}
