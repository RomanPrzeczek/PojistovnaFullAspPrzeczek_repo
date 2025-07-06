using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PojistovnaFullAspPrzeczek.Data;
using PojistovnaFullAspPrzeczek.Interfaces;
using PojistovnaFullAspPrzeczek.Repositories;
using PojistovnaFullAspPrzeczek.Services;
using PojistovnaFullAspPrzeczek.MappingProfiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// add services for services  ??
builder.Services.AddScoped<IInsuredPersonService, InsuredPersonService>(); // service for insured
builder.Services.AddScoped<IPersonInsuranceService, PersonInsuranceService>(); // service for insured insurances
builder.Services.AddScoped<IInsuranceService, InsuranceService>(); // service for insurances

// Add AutoMapper for mapping between models and DTOs
builder.Services.AddAutoMapper(
    typeof(InsuranceProfile),
    typeof(InsuredPersonProfile),
    typeof(PersonInsuranceProfile)
);

// Add services for dependency injection of repositories
builder.Services.AddScoped(typeof(IRepository<>),typeof(GenericRepository<>)); // generic repository
builder.Services.AddScoped<IInsuredPersonRepository,InsuredPersonRepository>(); // specific InsuredPerson repository
builder.Services.AddScoped<IInsuranceRepository, InsuranceRepository>(); // specific Insurance repository
builder.Services.AddScoped<IPersonInsuranceRepository, PersonInsuranceRepository>(); // specific PersonInsurance repository

// Configure the database connection string based on the environment
var connectionString = builder.Configuration.GetConnectionString("PostgreSqlConnection")
    ?? builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("No valid connection string found.");

var dbProvider = builder.Configuration.GetValue<string>("DatabaseProvider");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    switch (dbProvider)
    {
        case "Sqlite":
            options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection"));
            break;
        case "SqlServer":
            options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection"));
            break;
        case "PostgreSql":
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("PostgreSqlConnection")
                ?? builder.Configuration.GetConnectionString("DefaultConnection")
            );
            break;
        default:
            throw new InvalidOperationException("Unsupported database provider: " + dbProvider);
    }
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()   // service pro role
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services
    .AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

var app = builder.Build();

var supportedCultures = new[] { "cs", "en" };

// lokalizace musí být použita pøed routingem
var localizationOptions = new RequestLocalizationOptions() { ApplyCurrentCultureToResponseHeaders = true }
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

// Pouze v Development prostøedí vymaže data, kromnì test-admina a -pojištìnce
/*if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    SeedData.ClearDevData(services);
    await SeedData.InitializeAsync(services);
}
*/
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// middleware pipeline:
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// vytvoøení rolí
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roles = new[] { "admin", "insuredUser", "general" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

app.Run();
