using AutoMapper;
using PojistovnaFullAspPrzeczek.Models;
using PojistovnaFullAspPrzeczek.DTOs.Insurances;
using PojistovnaFullAspPrzeczek.DTOs.InsuredPersons;
using PojistovnaFullAspPrzeczek.DTOs.PersonInsurances;

namespace PojistovnaFullAspPrzeczek.MappingProfiles
{
    public class InsuredPersonProfile : Profile
    {
        public InsuredPersonProfile()
        {
            CreateMap<InsuredPerson, InsuredPersonDto>()
                // AutoMapper maps every DTO expressed attributes acc name automatically
                // While FullName does not exist in DTO (is made separately in profile), thus has to be mapped manually from Name + Surname
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.Name} {src.Surname}"));
            CreateMap<InsuredPerson, InsuredPersonDetailDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.Name} {src.Surname}"));

            CreateMap<InsuredPerson, InsuredPersonCreateDto>();
            CreateMap<InsuredPersonCreateDto, InsuredPerson>();
            
            CreateMap<InsuredPerson, InsuredPersonEditDto>();
            CreateMap<InsuredPersonEditDto, InsuredPerson>();
            
            CreateMap<InsuredPerson, InsuredPersonDeleteDto>();

            CreateMap<PersonInsurance, PersonInsuranceDto>();
            CreateMap<Insurance, InsuranceDto>();
        }
    }
}
