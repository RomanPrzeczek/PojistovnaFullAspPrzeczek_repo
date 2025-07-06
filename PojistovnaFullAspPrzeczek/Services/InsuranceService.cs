using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PojistovnaFullAspPrzeczek.Data;
using PojistovnaFullAspPrzeczek.DTOs.Insurances;
using PojistovnaFullAspPrzeczek.Interfaces;
using PojistovnaFullAspPrzeczek.Models;

namespace PojistovnaFullAspPrzeczek.Services
{
    public class InsuranceService : IInsuranceService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public InsuranceService(ApplicationDbContext applicationDbContext, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _context = applicationDbContext;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task CreateAsync(InsuranceDto dto)
        {
            var entity = _mapper.Map<Insurance>(dto);
            _context.Insurance.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Insurance.FindAsync(id);
            if (entity == null)
                throw new ArgumentException("Pojištění nenalezeno.");

            _context.Insurance.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<InsuranceDto>> GetAllAsync()
        {
            var entities = await _context.Insurance.ToListAsync();
            return _mapper.Map<List<InsuranceDto>>(entities);
        }

        public async Task<InsuranceDto?> GetByIdAsync(int id)
        {
            var entity = await _context.Insurance
                .FirstOrDefaultAsync(p => p.IdInsurance == id);

            return entity == null ? null : _mapper.Map<InsuranceDto>(entity);
        }

        public Task<InsuranceDto> GetCreateTemplateAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<InsuranceDto?> GetDeleteByIdAsync(int id)
        {
            var entity = await _context.Insurance.FindAsync(id);
            return entity == null ? null : _mapper.Map<InsuranceDto>(entity);
        }

        public async Task<InsuranceDto?> GetEditByIdAsync(int id)
        {
            var entity = await _context.Insurance.FindAsync(id);
            return entity == null ? null : _mapper.Map<InsuranceDto>(entity);
        }

        public async Task UpdateAsync(InsuranceDto dto)
        {
            var entity = await _context.Insurance.FindAsync(dto.IdInsurance);
            if (entity == null)
                throw new ArgumentException("Pojištění nenalezeno.");

            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();
        }
    }
}
