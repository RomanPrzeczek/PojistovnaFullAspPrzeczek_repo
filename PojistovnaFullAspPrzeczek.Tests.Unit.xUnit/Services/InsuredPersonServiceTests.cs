using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using PojistovnaFullAspPrzeczek.Data;
using PojistovnaFullAspPrzeczek.Models;
using PojistovnaFullAspPrzeczek.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PojistovnaFullAspPrzeczek.Tests.Unit.xUnit.Services
{
    public class InsuredPersonServiceTests
    {
        private readonly IMapper _mapper;

        public InsuredPersonServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                /// option with true InsuredPersonProfile production class = true mapping
                cfg.AddProfile<PojistovnaFullAspPrzeczek.MappingProfiles.InsuredPersonProfile>();

                /// option with manual test-mapping = building DTOs FullName (from Model's Name+Surname) manually here
/*                cfg.CreateMap<InsuredPerson, InsuredPersonDto>()
                    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.Name} {src.Surname}"));
*/
            });

            _mapper = config.CreateMapper();
        }

        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + System.Guid.NewGuid())
                .Options;

            return new ApplicationDbContext(options);
        }


        [Fact]
        public async Task GetAllAsync_ReturnsAllInsuredPersons()
        {
            // Arrange
            using var context = GetInMemoryDbContext();

            context.InsuredPerson.AddRange(new[]
            {
            new InsuredPerson { IdInsuredPerson = 1, Name = "Adam", Surname = "Testový", Email = "adam@example.com" },
            new InsuredPerson { IdInsuredPerson = 2, Name = "Eva", Surname = "Testová", Email = "eva@example.com" }
            });

            await context.SaveChangesAsync();

            var userManagerMock = new Mock<Microsoft.AspNetCore.Identity.UserManager<Microsoft.AspNetCore.Identity.IdentityUser>>(
                Mock.Of<Microsoft.AspNetCore.Identity.IUserStore<Microsoft.AspNetCore.Identity.IdentityUser>>(), null, null, null, null, null, null, null, null);

            var service = new InsuredPersonService(context, _mapper, userManagerMock.Object);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, p => p.FullName == "Eva Testová");
            Assert.Contains(result, p => p.Email == "eva@example.com");
        }

    }
}
