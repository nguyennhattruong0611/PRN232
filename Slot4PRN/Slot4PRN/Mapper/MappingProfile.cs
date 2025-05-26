using AutoMapper;
using Slot4PRN.DTO;
using Slot4PRN.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Slot4PRN.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyDto>()
                .ForMember(
                    c => c.FullAddress,
                    opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country))
                );

            CreateMap<Employee, EmployeeDto>();

            CreateMap<CompanyForCreationDto, Company>();

            CreateMap<EmployeeForCreationDto, Employee>();

            CreateMap<EmployeeForUpdateDto, Employee>()
                .ReverseMap();

            CreateMap<CompanyForUpdateDto, Company>();
        }
    }
}
