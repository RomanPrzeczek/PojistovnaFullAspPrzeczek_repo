using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PojistovnaFullAspPrzeczek.Models;
using System;
using System.Threading.Tasks;

namespace PojistovnaFullAspPrzeczek.Data
{
    public class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            // Role
            string adminRole = "Admin";
            string insuredRole = "InsuredPerson";

            // Admin info
            string adminEmail = "admin@example.com";
            string adminPassword = "Admin123+";

            // Insured user info
            string userEmail = "user@example.com";
            string userPassword = "User123+";

            // 1. Role creation
            foreach (var roleName in new[] { adminRole, insuredRole })
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // 2. Admin creation
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (!result.Succeeded)
                    throw new Exception("Chyba při vytváření admina: " + string.Join("; ", result.Errors));
            }
            if (!await userManager.IsInRoleAsync(adminUser, adminRole))
            {
                await userManager.AddToRoleAsync(adminUser, adminRole);
            }

            // 3. Insured user creation
            var insuredUser = await userManager.FindByEmailAsync(userEmail);
            if (insuredUser == null)
            {
                insuredUser = new IdentityUser
                {
                    UserName = userEmail,
                    Email = userEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(insuredUser, userPassword);
                if (!result.Succeeded)
                    throw new Exception("Chyba při vytváření uživatele: " + string.Join("; ", result.Errors));
            }
            if (!await userManager.IsInRoleAsync(insuredUser, insuredRole))
            {
                await userManager.AddToRoleAsync(insuredUser, insuredRole);
            }

            // 4. InsuredPerson napojení
            var existingInsured = context.InsuredPerson.FirstOrDefault(p => p.Email == userEmail);
            if (existingInsured == null)
            {
                var insured = new InsuredPerson
                {
                    Name = "Jan",
                    Surname = "Testovací",
                    City = "TestCity",
                    Street = "TesStreet 1",
                    ZIP = 12345,
                    Phone = "123456789",
                    Email = userEmail
                };
                context.InsuredPerson.Add(insured);
                await context.SaveChangesAsync();
            }
        }
        public static void ClearDevData(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // E-maily uživatelů, které chceme zachovat
            var keepEmails = new[] { "admin@example.com", "user@example.com" };

            // 1. Smazání PersonInsurance záznamů kromě těch navázaných na uživatele s keepEmails
            var keepInsuredIds = context.InsuredPerson
                .Where(p => keepEmails.Contains(p.Email))
                .Select(p => p.IdInsuredPerson)
                .ToHashSet();

            var deletePersonInsurances = context.PersonInsurance
                .Where(pi => !keepInsuredIds.Contains(pi.IdPersonInsurance))
                .ToList();

            context.PersonInsurance.RemoveRange(deletePersonInsurances);

            // 2. Smazání InsuredPerson záznamů kromě těch s keepEmails
            var deleteInsuredPersons = context.InsuredPerson
                .Where(p => !keepEmails.Contains(p.Email))
                .ToList();

            context.InsuredPerson.RemoveRange(deleteInsuredPersons);

            // 3. Smazání IdentityUser záznamů kromě těch s keepEmails
            var deleteUsers = context.Users
                .Where(u => !keepEmails.Contains(u.Email))
                .ToList();

            context.Users.RemoveRange(deleteUsers);

            context.SaveChanges();
        }
    }
}