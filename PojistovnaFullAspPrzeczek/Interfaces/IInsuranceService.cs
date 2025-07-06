using PojistovnaFullAspPrzeczek.DTOs.Insurances;

namespace PojistovnaFullAspPrzeczek.Interfaces
{
    public interface IInsuranceService
    {
        Task<List<InsuranceDto>> GetAllAsync();
        Task<InsuranceDto?> GetByIdAsync(int id);
        Task<InsuranceDto> GetCreateTemplateAsync(); 
        Task CreateAsync(InsuranceDto dto);
        Task<InsuranceDto?> GetEditByIdAsync(int id);
        Task UpdateAsync(InsuranceDto dto);
        Task<InsuranceDto?> GetDeleteByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
