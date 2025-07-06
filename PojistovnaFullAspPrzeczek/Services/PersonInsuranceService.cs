using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PojistovnaFullAspPrzeczek.Data;
using PojistovnaFullAspPrzeczek.DTOs.InsuredPersons;
using PojistovnaFullAspPrzeczek.DTOs.PersonInsurances;
using PojistovnaFullAspPrzeczek.Interfaces;
using AutoMapper;
using PojistovnaFullAspPrzeczek.DTOs.Insurances;
using PojistovnaFullAspPrzeczek.Models;

namespace PojistovnaFullAspPrzeczek.Services
{
    public class PersonInsuranceService : IPersonInsuranceService
    {
        private readonly ApplicationDbContext _context;

        private readonly IMapper _mapper;
        public PersonInsuranceService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        /// <summary>
        /// ChangeInsurance of isuredPerson, get, post
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InsuredPersonChangeInsuranceDto?> GetSelectableForInsuredPerson(int id)
        {
            var insured = await _context.InsuredPerson
                .FirstOrDefaultAsync(p => p.IdInsuredPerson == id);

            var insuredId = insured?.IdInsuredPerson;

            if (insured == null) return null;

            var insurances = await _context.PersonInsurance
                .Where(pi => pi.IdInsuredPerson != insuredId) // only insurances not assigned to this insured person
                .Include(pi => pi.Insurance)
                .ToListAsync();

            var dto = new InsuredPersonChangeInsuranceDto
            {
                IdInsuredPerson = insured.IdInsuredPerson,
                FullName = insured.Name + " " + insured.Surname,
                PersonInsurances = insurances
                    .Select(pi => new SelectListItem
                    {
                        Value = pi.IdPersonInsurance.ToString(),
                        Text = pi.SubjectOfInsurance
                    }).ToList()
            };

            return dto;
        }

        public async Task<bool> AssignInsuranceToInsuredPerson(int insuredId, int personInsuranceId)
        {
            var insured = await _context.InsuredPerson
                .Include(p => p.PersonInsurances)
                .FirstOrDefaultAsync(p => p.IdInsuredPerson == insuredId);

            if (insured == null) return false;

            var personInsurance = await _context.PersonInsurance
                .FirstOrDefaultAsync(pi => pi.IdPersonInsurance == personInsuranceId);

            if (personInsurance == null) return false;

            var isAlreadyAssigned = insured.PersonInsurances.Any(pi => pi.IdPersonInsurance == personInsuranceId);
            if (isAlreadyAssigned)
            {
                throw new InvalidOperationException("Toto pojištění již bylo přiděleno tomuto pojištěnci.");
            }

            personInsurance.IdInsuredPerson = insuredId;
            personInsurance.InsuredPerson = insured;

            // securing adding insurance to insured collection for memory conzistence 
            insured.PersonInsurances.Add(personInsurance);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<PersonInsuranceDto>> GetAllAsync()
        {
            var entities = await _context.PersonInsurance.ToListAsync();
            return _mapper.Map<List<PersonInsuranceDto>>(entities);
        }

        public async Task<PersonInsuranceDto?> GetByIdAsync(int id)
        {
            var entity = await _context.PersonInsurance
                .FirstOrDefaultAsync(p => p.IdPersonInsurance == id);

            return entity == null ? null : _mapper.Map<PersonInsuranceDto>(entity);
        }

        public Task<PersonInsuranceDto> GetCreateTemplateAsync()
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(PersonInsuranceCreateDto dto)
        {
            var entity = _mapper.Map<PersonInsurance>(dto);

            // Explicitně nastav vztah (kvůli pozdějšímu výpisu a navigaci)
            entity.IdInsuredPerson = dto.IdInsuredPerson;
            entity.StartDate = DateTime.SpecifyKind(dto.StartDate, DateTimeKind.Utc);
            entity.EndDate = DateTime.SpecifyKind(dto.EndDate, DateTimeKind.Utc);

            // Můžeš volitelně načíst i pojištěnce a navázat objekt
            var insured = await _context.InsuredPerson.FindAsync(dto.IdInsuredPerson);
            if (insured != null)
            {
                entity.InsuredPerson = insured; // ⚠️ důležité pro výpis přes Include()
            }

            _context.PersonInsurance.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<PersonInsuranceDto?> GetEditByIdAsync(int id)
        {
            var entity = await _context.PersonInsurance.FindAsync(id);
            return entity == null ? null : _mapper.Map<PersonInsuranceDto>(entity);
        }

        public async Task UpdateAsync(PersonInsuranceDto dto)
        {
            var entity = await _context.PersonInsurance.FindAsync(dto.IdPersonInsurance);
            if (entity == null)
                throw new ArgumentException("Pojištění nenalezeno.");

            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();
        }

        public async Task<PersonInsuranceDto?> GetDeleteByIdAsync(int id)
        {
            var entity = await _context.PersonInsurance.FindAsync(id);
            return entity == null ? null : _mapper.Map<PersonInsuranceDto>(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.PersonInsurance.FindAsync(id);
            if (entity == null)
                throw new ArgumentException("Pojištění nenalezeno.");

            _context.PersonInsurance.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
