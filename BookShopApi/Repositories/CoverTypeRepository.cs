using BookShopApi.Data;
using BookShopApi.Dtos.CoverType;
using BookShopApi.Interfaces;
using BookShopApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShopApi.Repositories
{
    public class CoverTypeRepository : ICoverTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public CoverTypeRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<IEnumerable<CoverType>> GetAllCoverTypesAsync()
        {
            return await _context.CoverTypes.ToListAsync();
        }

        public async Task<CoverType?> GetCoverTypeByIdAsync(int id)
        {
            return await _context.CoverTypes.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<CoverType> CreateCoverTypeAsync(CoverType coverType)
        {
            await _context.CoverTypes.AddAsync(coverType);
            await _context.SaveChangesAsync();
            return coverType;
        }

        public async Task<CoverType?> UpdateCoverTypeAsync(UpdateCoverTypeDto coverTypeDto, int id)
        {
            var coverType = await _context.CoverTypes.FirstOrDefaultAsync(c => c.Id == id);

            if (coverType == null)
                return null;

            coverType.Name = coverTypeDto.Name;

            await _context.SaveChangesAsync();
            return coverType;
        }

        public async Task<CoverType?> DeleteCoverTypeAsync(int id)
        {
            var coverType = await _context.CoverTypes.FirstOrDefaultAsync(c => c.Id == id);

            if (coverType == null)
                return null;

            _context.CoverTypes.Remove(coverType);
            await _context.SaveChangesAsync();
            return coverType;
        }
    }
}
