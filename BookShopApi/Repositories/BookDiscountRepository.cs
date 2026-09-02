using BookShopApi.Data;
using BookShopApi.Dtos.BookDiscount;
using BookShopApi.Interfaces;
using BookShopApi.Mappers;
using BookShopApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShopApi.Repositories
{
    public class BookDiscountRepository : IBookDiscountRepository
    {
        private readonly ApplicationDbContext _context;

        public BookDiscountRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<IEnumerable<BookDiscount>> GetByBookIdAsync(int bookId)
        {
            return await _context.BookDiscounts
                .Where(d => d.BookId == bookId)
                .OrderByDescending(d => d.Id)
                .ToListAsync();
        }

        public async Task<BookDiscount?> GetByIdAsync(int bookId, int id)
        {
            return await _context.BookDiscounts
                .FirstOrDefaultAsync(d => d.Id == id && d.BookId == bookId);
        }

        public async Task<BookDiscount> CreateAsync(int bookId, CreateBookDiscountDto dto)
        {
            if (!await _context.Books.AnyAsync(b => b.Id == bookId))
                throw new ArgumentException($"The book with Id: {bookId}, Not found.");

            if (dto.IsActive)
                await DeactivateActiveDiscountsAsync(bookId);

            var discount = dto.ToBookDiscountFromCreateDto(bookId);
            await _context.BookDiscounts.AddAsync(discount);
            await _context.SaveChangesAsync();
            return discount;
        }

        public async Task<BookDiscount?> UpdateAsync(int bookId, int id, UpdateBookDiscountDto dto)
        {
            var discount = await GetByIdAsync(bookId, id);
            if (discount == null)
                return null;

            if (dto.IsActive)
                await DeactivateActiveDiscountsAsync(bookId, exceptId: id);

            discount.SetDataToBookDiscountFromUpdateDto(dto);
            await _context.SaveChangesAsync();
            return discount;
        }

        public async Task<BookDiscount?> DeactivateAsync(int bookId, int id)
        {
            var discount = await GetByIdAsync(bookId, id);
            if (discount == null)
                return null;

            discount.IsActive = false;
            await _context.SaveChangesAsync();
            return discount;
        }

        private async Task DeactivateActiveDiscountsAsync(int bookId, int? exceptId = null)
        {
            var others = await _context.BookDiscounts
                .Where(d => d.BookId == bookId && d.IsActive && (exceptId == null || d.Id != exceptId))
                .ToListAsync();

            foreach (var other in others)
                other.IsActive = false;
        }
    }
}
