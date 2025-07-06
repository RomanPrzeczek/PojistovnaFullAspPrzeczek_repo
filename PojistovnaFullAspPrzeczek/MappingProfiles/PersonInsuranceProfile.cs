using AutoMapper;
using PojistovnaFullAspPrzeczek.DTOs.PersonInsurances;
using PojistovnaFullAspPrzeczek.Models;

namespace PojistovnaFullAspPrzeczek.MappingProfiles
{
    public class PersonInsuranceProfile : Profile
    {
        public PersonInsuranceProfile()
        {
            CreateMap<PersonInsurance, PersonInsuranceDto>();
            CreateMap<PersonInsuranceDto, PersonInsurance>();

            CreateMap<PersonInsurance, PersonInsuranceCreateDto>();
            CreateMap<PersonInsuranceCreateDto, PersonInsurance>();
        }
    }
}
