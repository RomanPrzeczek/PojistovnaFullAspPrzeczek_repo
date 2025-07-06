using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PojistovnaFullAspPrzeczek.Models;

namespace PojistovnaFullAspPrzeczek.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

public DbSet<PojistovnaFullAspPrzeczek.Models.Insurance> Insurance { get; set; } = default!;

public DbSet<PojistovnaFullAspPrzeczek.Models.PersonInsurance> PersonInsurance { get; set; } = default!;

public DbSet<PojistovnaFullAspPrzeczek.Models.InsuredPerson> InsuredPerson { get; set; } = default!;

}
