using AutoMapper;
using PojistovnaFullAspPrzeczek.DTOs.Insurances;
using PojistovnaFullAspPrzeczek.Models;

namespace PojistovnaFullAspPrzeczek.MappingProfiles
{
    public class InsuranceProfile : Profile
    {
        public InsuranceProfile()
        {
            CreateMap<Insurance, InsuranceDto>();
            CreateMap<InsuranceDto, Insurance>();
        }
    }
}
