using PojistovnaFullAspPrzeczek.Models;

namespace PojistovnaFullAspPrzeczek.Interfaces
{
    public interface IInsuredPersonRepository : IRepository<InsuredPerson>
    {
        Task<IEnumerable<InsuredPerson>> GetByCityAsync(string city);
    }
}
