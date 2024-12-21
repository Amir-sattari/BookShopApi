using BookShopApi.Data;
using BookShopApi.Dtos.Discount;
using BookShopApi.Interfaces;
using BookShopApi.Mappers;
using BookShopApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShopApi.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly ApplicationDbContext _context;

        public DiscountRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<ICollection<Discount>> GetAllDiscountsAsync()
        {
            return await _context.Discounts.ToListAsync();
        }

        public async Task<ICollection<Discount>> GetAllActiveDiscountsAsync()
        {
            return await _context.Discounts.Where(d => d.StartDate <= DateTime.Now && d.EndDate >= DateTime.Now).ToListAsync();
        }

        public async Task<Discount?> GetSingleActiveDiscountByNameAsync(string discountName)
        {
            return await _context.Discounts.FirstOrDefaultAsync(d => d.Name == discountName && d.StartDate <= DateTime.Now && d.EndDate >= DateTime.Now);
        }

        public async Task<Discount?> GetDiscountByIdAsync(int id)
        {
            return await _context.Discounts.FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Discount?> GetDiscountByNameAsync(string discountName)
        {
            return await _context.Discounts.FirstOrDefaultAsync(d => d.Name == discountName);
        }

        public async Task<Discount> CreateDiscountAsync(CreateDiscountDto discountDto)
        {
            var endDate = CalculateEndDate(discountDto.StartDate, discountDto.Duration, discountDto.DurationUnit);

            var discount = discountDto.SetDataToDiscountFromCreateDto(endDate);

            await _context.Discounts.AddAsync(discount);
            await _context.SaveChangesAsync();
            return discount;
        }

        public async Task<Discount?> UpdateDiscountAsync(UpdateDiscountDto discountDto, int id)
        {
            var discount = await _context.Discounts.FirstOrDefaultAsync(d => d.Id == id);

            if (discount == null)
                return null;

            var endDate = CalculateEndDate(discountDto.StartDate, discountDto.Duration, discountDto.DurationUnit);
            discount.SetDataToDiscountFromUpdateDto(discountDto, endDate);
            await _context.SaveChangesAsync();
            return discount;
        }

        public async Task<Discount?> DeleteDiscountByIdAsync(int id)
        {
            var discount = await _context.Discounts.FirstOrDefaultAsync(d => d.Id == id);

            if (discount == null)
                return null;

            _context.Discounts.Remove(discount);
            await _context.SaveChangesAsync();
            return discount;
        }

        private static DateTime CalculateEndDate(DateTime startDate, int Duration, string DurationUnit)
        {
            switch (DurationUnit.ToLower())
            {
                case "minutes":
                    return startDate.AddMinutes(Duration);

                case "hours":
                    return startDate.AddHours(Duration);

                case "days":
                    return startDate.AddDays(Duration);

                default:
                    throw new ArgumentException("Invalid Duration Unit. use 'minutes', 'hours', or 'days' .");
            }
        }
    }
}
