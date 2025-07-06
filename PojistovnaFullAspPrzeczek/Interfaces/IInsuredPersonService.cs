using PojistovnaFullAspPrzeczek.DTOs.InsuredPersons;

namespace PojistovnaFullAspPrzeczek.Interfaces
{
    public interface IInsuredPersonService
    {
        Task<List<InsuredPersonDto>> GetAllAsync();
        Task<InsuredPersonDetailDto?> GetByIdAsync(int id);

        Task<InsuredPersonCreateDto> GetCreateTemplateAsync(); // pokud budeš mít třeba výchozí hodnoty
        Task CreateAsync(InsuredPersonCreateDto dto);

        Task<InsuredPersonEditDto?> GetEditByIdAsync(int id);
        Task UpdateAsync(InsuredPersonEditDto dto);

        Task<InsuredPersonDeleteDto?> GetDeleteByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
