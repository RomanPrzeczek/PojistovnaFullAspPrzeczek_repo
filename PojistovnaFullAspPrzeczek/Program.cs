using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PojistovnaFullAspPrzeczek.Data;
using PojistovnaFullAspPrzeczek.Interfaces;
using PojistovnaFullAspPrzeczek.Repositories;
using PojistovnaFullAspPrzeczek.Services;
using PojistovnaFullAspPrzeczek.MappingProfiles;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IInsuredPersonService, InsuredPersonService>(); 
builder.Services.AddScoped<IPersonInsuranceService, PersonInsuranceService>(); 
builder.Services.AddScoped<IInsuranceService, InsuranceService>(); 

// Add AutoMapper for mapping between models and DTOs
builder.Services.AddAutoMapper(
    typeof(InsuranceProfile),
    typeof(InsuredPersonProfile),
    typeof(PersonInsuranceProfile)
);

// Add services for dependency injection of repositories
builder.Services.AddScoped(typeof(IRepository<>),typeof(GenericRepository<>)); 
builder.Services.AddScoped<IInsuredPersonRepository,InsuredPersonRepository>(); 
builder.Services.AddScoped<IInsuranceRepository, InsuranceRepository>(); 
builder.Services.AddScoped<IPersonInsuranceRepository, PersonInsuranceRepository>(); 

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
    .AddRoles<IdentityRole>()   
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services
    .AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

var app = builder.Build();

var supportedCultures = new[] { "cs", "en" };

var localizationOptions = new RequestLocalizationOptions() { ApplyCurrentCultureToResponseHeaders = true }
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
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

// testing middleware environment
if (app.Environment.IsEnvironment("Testing"))
{
    app.UseAuthentication();
    app.UseAuthorization();
}

// roles creation
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

public partial class Program { } // for integration test WebApplicationFactory