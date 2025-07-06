using PojistovnaFullAspPrzeczek.Interfaces;
using PojistovnaFullAspPrzeczek.Models;
using PojistovnaFullAspPrzeczek.Data;
using Microsoft.EntityFrameworkCore;

namespace PojistovnaFullAspPrzeczek.Repositories
{
    public class InsuredPersonRepository : GenericRepository<InsuredPerson>, IInsuredPersonRepository
    {
        public InsuredPersonRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<InsuredPerson>> GetByCityAsync(string city)
        {
            return await _dbSet
                .Where(p => p.City.ToLower() == city.ToLower())
                .ToListAsync();
        }
    }
}
