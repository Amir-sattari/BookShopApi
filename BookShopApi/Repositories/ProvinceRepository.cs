using BookShopApi.Data;
using BookShopApi.Dtos.Province;
using BookShopApi.Interfaces;
using BookShopApi.Mappers;
using BookShopApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShopApi.Repositories
{
    public class ProvinceRepository : IProvinceRepository
    {
        private readonly ApplicationDbContext _context;

        public ProvinceRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<ICollection<Province>> GetProvincesAsync()
        {
            return await _context.Provinces.ToListAsync();
        }
        public async Task<Province?> GetProvinceByIdAsync(int id)
        {
            return await _context.Provinces.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Province> CreateProvinceAsync(CreateProvinceDto provinceDto)
        {
            var province = provinceDto.SetDataToProvinceFromCreateDto();

            await _context.Provinces.AddAsync(province);
            await _context.SaveChangesAsync();
            return province;
        }
        public async Task<Province?> UpdateProvinceAsync(UpdateProvinceDto provinceDto, int id)
        {
            var province = await _context.Provinces.FirstOrDefaultAsync(p => p.Id == id);

            if (province == null)
                return null;

            province.SetDataToProvinceFromUpdateDto(provinceDto);
            await _context.SaveChangesAsync();
            return province;
        }

        public async Task<Province?> DeleteProvinceAsync(int id)
        {
            var province = await _context.Provinces.FirstOrDefaultAsync(p => p.Id == id);

            if (province == null)
                return null;

            _context.Provinces.Remove(province);
            await _context.SaveChangesAsync();
            return province;
        }
    }
}
