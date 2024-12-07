using BookShopApi.Data;
using BookShopApi.Dtos.ShippingAddress;
using BookShopApi.Interfaces;
using BookShopApi.Mappers;
using BookShopApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShopApi.Repositories
{
    public class ShippingAddressRepository : IShippingAddressRepository
    {
        private readonly ApplicationDbContext _context;

        public ShippingAddressRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<ICollection<ShippingAddress>> GetAllShippingAddressesAsync()
        {
            return await _context.ShippingAddresses.ToListAsync();
        }

        public async Task<ShippingAddress?> GetShippingAddressByUserIdAsync(string userId)
        {
            var isUserIdExist = await _context.Users.AnyAsync(u => u.Id == userId);

            if (!isUserIdExist)
                throw new InvalidOperationException("The UserId Is Invalid or Doesn't Exist.");

            return await _context.ShippingAddresses.FirstOrDefaultAsync(sh => sh.UserId == userId);
        }

        public async Task<ShippingAddress> CreateShippingAddressAsync(CreateShippingAddressDto addressDto)
        {
            var isUserIdExist = await _context.Users.AnyAsync(u => u.Id == addressDto.UserId);

            if (!isUserIdExist)
                throw new InvalidOperationException("The UserId Is Invalid or Doesn't Exist.");

            var shippingAddress = addressDto.SetDataToShippingAddressFromCreateDto();

            await _context.ShippingAddresses.AddAsync(shippingAddress);
            await _context.SaveChangesAsync();
            return shippingAddress;
        }

        public async Task<ShippingAddress?> UpdateShippingAddressAsync(UpdateShippingAddressDto addressDto, string userId)
        {
            var existingAddress = await _context.ShippingAddresses.FirstOrDefaultAsync(sh => sh.UserId == userId);

            if (existingAddress == null)
                return null;

            existingAddress.SetDataToShippingAddressFromUpdateDto(addressDto, userId);
            await _context.SaveChangesAsync();
            return existingAddress;
        }

        public async Task<ShippingAddress?> DeleteShippingAddressByUserIdAsync(string userId)
        {
            var existingAddress = await _context.ShippingAddresses.FirstOrDefaultAsync(sh => sh.UserId == userId);

            if (existingAddress == null)
                return null;

            _context.ShippingAddresses.Remove(existingAddress);
            await _context.SaveChangesAsync();
            return existingAddress;
        }
    }
}
