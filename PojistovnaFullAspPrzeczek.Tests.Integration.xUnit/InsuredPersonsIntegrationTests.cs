using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Xunit;
using PojistovnaFullAspPrzeczek.Data;
using PojistovnaFullAspPrzeczek;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.AspNetCore.Hosting;
using PojistovnaFullAspPrzeczek.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.TestHost;

namespace PojistovnaFullAspPrzeczek.Tests.Integration.xUnit
{
    public class InsuredPersonsIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public InsuredPersonsIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");

                builder.ConfigureServices(services =>
                {
                    // ⛔️ removes original PostgreSQL configuration
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // ✅ adds InMemory DB
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDb");
                    });

                    // ✅ data seeding
                    using var scope = services.BuildServiceProvider().CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    db.Database.EnsureCreated();

                    db.InsuredPerson.Add(new InsuredPerson
                    {
                        Name = "Romi",
                        Surname = "Test",
                        Email = "admin@example.com"
                    });

                    db.SaveChanges();
                });

                builder.ConfigureTestServices(services =>
                {
                    // ✅ add fake authentication handler for testing
                    services.AddAuthentication("TestAuth")
                        .AddScheme<AuthenticationSchemeOptions, FakeAuthHandler>("TestAuth", options => { });
                });
            });
        }

        [Fact]
        public async Task Get_InsuredPersons_Index_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Simulate authentication
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestAuth");

            // Act
            var response = await client.GetAsync("/InsuredPersons");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var html = await response.Content.ReadAsStringAsync();
            Assert.Contains("Romi", html); // hledá se jméno ve výstupu
        }
    }
}
