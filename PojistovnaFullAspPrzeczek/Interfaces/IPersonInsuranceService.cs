using PojistovnaFullAspPrzeczek.DTOs.Insurances;
using PojistovnaFullAspPrzeczek.DTOs.InsuredPersons;
using PojistovnaFullAspPrzeczek.DTOs.PersonInsurances;

namespace PojistovnaFullAspPrzeczek.Interfaces
{
    public interface IPersonInsuranceService
    {
        Task<InsuredPersonChangeInsuranceDto?> GetSelectableForInsuredPerson(int id); 
        Task<bool> AssignInsuranceToInsuredPerson(int insuredId, int personInsuranceId);
        Task<List<PersonInsuranceDto>> GetAllAsync();
        Task<PersonInsuranceDto?> GetByIdAsync(int id);
        Task<PersonInsuranceDto> GetCreateTemplateAsync();
        Task CreateAsync(PersonInsuranceCreateDto dto);
        Task<PersonInsuranceDto?> GetEditByIdAsync(int id);
        Task UpdateAsync(PersonInsuranceDto dto);
        Task<PersonInsuranceDto?> GetDeleteByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
