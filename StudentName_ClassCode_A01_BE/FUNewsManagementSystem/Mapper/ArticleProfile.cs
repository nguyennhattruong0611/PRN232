using AutoMapper;
using BusinessObjects.DTO;
using BusinessObjects.Models;

namespace FUNewsManagementSystem.Mapper
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<StaffCreateArticleDto, NewsArticle>()
                .ForMember(dest => dest.NewsTags, opt => opt.Ignore()); // Sẽ xử lý NewsTags riêng

            // Mapping từ DTO sang Entity cho việc cập nhật bài viết
            CreateMap<StaffUpdateArticleDto, NewsArticle>()
                .ForMember(dest => dest.NewsTags, opt => opt.Ignore()); // Sẽ xử lý NewsTags riêng

            // Mapping từ Entity sang DTO để hiển thị
            CreateMap<NewsArticle, ArticleViewDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : string.Empty))
                .ForMember(dest => dest.CreatedByAccountName, opt => opt.MapFrom(src => src.CreatedBy != null ? src.CreatedBy.AccountName : string.Empty))
                .ForMember(dest => dest.UpdatedByAccountName, opt => opt.MapFrom(src => src.UpdatedBy != null ? src.UpdatedBy.AccountName : string.Empty))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.NewsTags.Select(nt => nt.Tag).ToList())); // Map danh sách Tag

            CreateMap<Tag, TagViewDto>(); // Mapping cho TagViewDto
        }
    }
}
