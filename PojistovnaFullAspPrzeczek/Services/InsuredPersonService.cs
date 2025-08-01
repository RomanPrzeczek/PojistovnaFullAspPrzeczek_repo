using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PojistovnaFullAspPrzeczek.Data;
using PojistovnaFullAspPrzeczek.DTOs.InsuredPersons;
using PojistovnaFullAspPrzeczek.Interfaces;
using PojistovnaFullAspPrzeczek.Models;

namespace PojistovnaFullAspPrzeczek.Services
{
    public class InsuredPersonService : IInsuredPersonService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public InsuredPersonService(ApplicationDbContext context, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }
        /// <summary>
        /// Read, all, one
        /// </summary>
        /// <returns></returns>
        public async Task<List<InsuredPersonDto>> GetAllAsync()
        {
            var entities = await _context.InsuredPerson.ToListAsync();
            return _mapper.Map<List<InsuredPersonDto>>(entities);
        }

        public async Task<InsuredPersonDetailDto?> GetByIdAsync(int id)
        {
            var entity = await _context.InsuredPerson
                .Include(p => p.PersonInsurances)
                .ThenInclude(pi => pi.Insurance)
                .FirstOrDefaultAsync(p => p.IdInsuredPerson == id);

            return entity == null ? null : _mapper.Map<InsuredPersonDetailDto>(entity);
        }
        /// <summary>
        /// Create, get not used sofar, post used
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<InsuredPersonCreateDto> GetCreateTemplateAsync() 
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(InsuredPersonCreateDto dto)
        {
            var entity = _mapper.Map<InsuredPerson>(dto);
            _context.InsuredPerson.Add(entity);
            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// Delete, get, post
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InsuredPersonDeleteDto?> GetDeleteByIdAsync(int id)
        {
            var entity = await _context.InsuredPerson.FindAsync(id);
            return entity == null ? null : _mapper.Map<InsuredPersonDeleteDto>(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.InsuredPerson.FindAsync(id);
            if (entity == null)
                throw new ArgumentException("Pojištěnec nenalezen.");

            _context.InsuredPerson.Remove(entity);
            await _context.SaveChangesAsync();

            var identityUser = await _userManager.FindByEmailAsync(entity.Email);
            if (identityUser != null)
            {
                var result = await _userManager.DeleteAsync(identityUser);
                if (!result.Succeeded)
                {
                    throw new Exception("Nepodařilo se smazat identitu pojištěnce.");
                }
            }
        }
        /// <summary>
        /// Edit, get, post
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InsuredPersonEditDto?> GetEditByIdAsync(int id)
        {
            var entity = await _context.InsuredPerson.FindAsync(id);
            return entity == null ? null : _mapper.Map<InsuredPersonEditDto>(entity);
        }

        public async Task UpdateAsync(InsuredPersonEditDto dto)
        {
            var entity = await _context.InsuredPerson.FindAsync(dto.IdInsuredPerson);
            if (entity == null)
                throw new ArgumentException("Pojištěnec nenalezen.");

            // e-mail change section
            if (entity.Email != dto.Email)
            {
                var existingUser = await _userManager.FindByEmailAsync(dto.Email);
                if (existingUser != null)
                {
                    throw new InvalidOperationException("Zadaný email je již používán jiným uživatelem.");
                }

                var identityUser = await _userManager.FindByEmailAsync(entity.Email);
                if (identityUser != null)
                {
                    identityUser.Email = dto.Email;
                    identityUser.UserName = dto.Email;

                    var updateResult = await _userManager.UpdateAsync(identityUser);
                    if (!updateResult.Succeeded)
                    {
                        var errorMsg = string.Join(", ", updateResult.Errors.Select(e => e.Description));
                        throw new Exception($"Chyba při aktualizaci uživatele: {errorMsg}");
                    }
                }
            }

            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();
        }

        public async Task<InsuredPersonDto?> GetByEmailAsync(string email)
        {
            var person = await _context.InsuredPerson
                .FirstOrDefaultAsync(p => p.Email == email);

            return person is null ? null : _mapper.Map<InsuredPersonDto>(person);
        }
    }
}
