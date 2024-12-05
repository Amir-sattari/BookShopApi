using BookShopApi.Data;
using BookShopApi.Dtos.City;
using BookShopApi.Interfaces;
using BookShopApi.Mappers;
using BookShopApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShopApi.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly ApplicationDbContext _context;

        public CityRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<ICollection<City>> GetcitiesAsync()
        {
            return await _context.Cities.ToListAsync();
        }

        public async Task<City?> GetCityByIdAsync(int id)
        {
            return await _context.Cities.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<ICollection<City>> GetCitiesByProvinceId(int provinceId)
        {
            return await _context.Cities.Where(c => c.ProvinceId == provinceId).ToListAsync();
        }

        public async Task<City> CreateCityAsync(CreateCityDto cityDto)
        {
            var isProvinceIdExist = await _context.Provinces.AnyAsync(p => p.Id == cityDto.ProvinceId);

            if (!isProvinceIdExist)
                throw new InvalidOperationException("The ProvinceId Doesn't Exist.");

            var city = cityDto.SetDataToCityFromCreateDto();

            await _context.Cities.AddAsync(city);
            await _context.SaveChangesAsync();
            return city;
        }
        public async Task<City?> UpdateCityAsync(UpdateCityDto cityDto, int id)
        {
            var isProvinceIdExist = await _context.Provinces.AnyAsync(p => p.Id == cityDto.ProvinceId);

            if (!isProvinceIdExist)
                throw new InvalidOperationException("The ProvinceId Doesn't Exist.");

            var city = await _context.Cities.FirstOrDefaultAsync(c => c.Id == id);

            if (city == null)
                return null;

            city.SetDataToCityFromUpdateDto(cityDto);
            await _context.SaveChangesAsync();
            return city;
        }

        public async Task<City?> DeleteCityAsync(int id)
        {
            var city = await _context.Cities.FirstOrDefaultAsync(c => c.Id == id);

            if (city == null)
                return null;

            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();
            return city;
        }
    }
}
