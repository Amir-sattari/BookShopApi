using BookShopApi.Data;
using BookShopApi.Dtos.ShippingMethod;
using BookShopApi.Interfaces;
using BookShopApi.Mappers;
using BookShopApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShopApi.Repositories
{
    public class ShippingMethodRepository : IShippingMethodRepository
    {
        private readonly ApplicationDbContext _context;

        public ShippingMethodRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<ICollection<ShippingMethod>> GetAllShippingMethodsAsync()
        {
            return await _context.ShippingMethods.ToListAsync();
        }

        public async Task<ShippingMethod?> GetShippingMethodByIdAsync(int id)
        {
            return await _context.ShippingMethods.FirstOrDefaultAsync(sh => sh.Id == id);
        }

        public async Task<ShippingMethod> CreateShippingMethodAsync(CreateShippingMethodDto methodDto)
        {
            var shippingMethod = methodDto.SetDataToShippingMethodFromCreateDto();

            await _context.ShippingMethods.AddAsync(shippingMethod);
            await _context.SaveChangesAsync();
            return shippingMethod;
        }

        public async Task<ShippingMethod?> UpdateShippingMethodAsync(UpdateShippingMethodDto methodDto, int id)
        {
            var shippingMethod = await _context.ShippingMethods.FirstOrDefaultAsync(sh => sh.Id == id);

            if (shippingMethod == null)
                return null;

            shippingMethod.SetDataToShippingMethodFromUpdateDto(methodDto);
            await _context.SaveChangesAsync();
            return shippingMethod;
        }

        public async Task<ShippingMethod?> DeleteShippingMethodAsync(int id)
        {
            var shippingMethod = await _context.ShippingMethods.FirstOrDefaultAsync(sh => sh.Id == id);

            if (shippingMethod == null)
                return null;

            _context.ShippingMethods.Remove(shippingMethod);
            await _context.SaveChangesAsync();
            return shippingMethod;
        }
    }
}
