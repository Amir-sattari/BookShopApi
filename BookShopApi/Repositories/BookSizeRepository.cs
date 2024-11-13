using BookShopApi.Data;
using BookShopApi.Dtos.BookSize;
using BookShopApi.Interfaces;
using BookShopApi.Mappers;
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

        public async Task<BookSize> CreateBookSizeAsync(CreateBookSizeDto bookSizeDto)
        {
            var book = bookSizeDto.SetDataToBookSizeFromCreateDto();
            await _context.BookSizes.AddAsync(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<BookSize?> UpdateBookSizeAsync(UpdateBookSizeDto bookSizeDto, int id)
        {
            var bookSizeModel = await _context.BookSizes.FirstOrDefaultAsync(b => b.Id == id);

            if (bookSizeModel == null)
                return null;

            bookSizeModel.SetDataToBookSizeFromUpdateDto(bookSizeDto);

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
