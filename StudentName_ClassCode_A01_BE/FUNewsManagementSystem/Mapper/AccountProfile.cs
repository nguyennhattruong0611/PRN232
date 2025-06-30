using AutoMapper;
using BusinessObjects.DTO;
using BusinessObjects.Models;

namespace FUNewsManagementSystem.Mapper
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            // Định nghĩa ánh xạ từ SystemAccount sang SystemAccountDTO
            CreateMap<SystemAccount, SystemAccountDTO>();

            // Định nghĩa ánh xạ từ CreateAccountDTO sang SystemAccount
            CreateMap<CreateAccountDTO, SystemAccount>()
                .ForMember(dest => dest.AccountPassword, opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.AccountPassword)));
        }
    }
}
