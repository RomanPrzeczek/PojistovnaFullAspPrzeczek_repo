using PojistovnaFullAspPrzeczek.Interfaces;
using PojistovnaFullAspPrzeczek.Models;
using PojistovnaFullAspPrzeczek.Data;

namespace PojistovnaFullAspPrzeczek.Repositories
{
    public class InsuranceRepository : GenericRepository<Insurance>, IInsuranceRepository
    {
        public InsuranceRepository(ApplicationDbContext context) : base(context) { }
    }
}
