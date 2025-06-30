using AutoMapper;
using BusinessObjects.DTO;
using BusinessObjects.Models;

namespace FUNewsManagementSystem.Mapper
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryViewDto>();
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();
        }
    }
}
