using BookShopApi.Data;
using BookShopApi.Dtos.BookSize;
using BookShopApi.Interfaces;
using BookShopApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShopApi.Repositories
{
    public class BookSizeRepository : IBookSizeRepository
    {
        private readonly ApplicationDbContext _context;

        public BookSizeRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<IEnumerable<BookSize>> GetAllBookSizeAsync()
        {
            return await _context.BookSizes.ToListAsync();
        }

        public async Task<BookSize?> GetBookSizeByIdAsync(int id)
        {
            return await _context.BookSizes.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<BookSize> CreateBookSizeAsync(BookSize bookSize)
        {
            await _context.BookSizes.AddAsync(bookSize);
            await _context.SaveChangesAsync();
            return bookSize;
        }

        public async Task<BookSize?> UpdateBookSizeAsync(UpdateBookSizeDto bookSize, int id)
        {
            var bookSizeModel = await _context.BookSizes.FirstOrDefaultAsync(b => b.Id == id);

            if (bookSizeModel == null)
                return null;

            bookSizeModel.Name = bookSize.Name;

            await _context.SaveChangesAsync();
            return bookSizeModel;
        }

        public async Task<BookSize?> DeleteBookSizeAsync(int id)
        {
            var bookSizeModel = await _context.BookSizes.FirstOrDefaultAsync(b => b.Id == id);

            if (bookSizeModel == null)
                return null;

            _context.BookSizes.Remove(bookSizeModel);
            await _context.SaveChangesAsync();
            return bookSizeModel;
        }
    }
}
