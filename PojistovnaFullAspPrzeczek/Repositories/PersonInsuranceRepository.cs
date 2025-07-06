using PojistovnaFullAspPrzeczek.Interfaces;
using PojistovnaFullAspPrzeczek.Models;
using PojistovnaFullAspPrzeczek.Data;

namespace PojistovnaFullAspPrzeczek.Repositories
{
    public class PersonInsuranceRepository : GenericRepository<PersonInsurance>, IPersonInsuranceRepository
    {
        public PersonInsuranceRepository(ApplicationDbContext context) : base(context)
        { }
    }
}
