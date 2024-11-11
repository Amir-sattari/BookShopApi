using BookShopApi.Data;
using BookShopApi.Dtos.Book;
using BookShopApi.Interfaces;
using BookShopApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShopApi.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Book> CreateBookAsync(Book book)
        {
            if (!await _context.Publications.AnyAsync(p => p.Id == book.PublicationId && !p.IsDeleted))
                throw new ArgumentException("Invalid or deleted publicationId");

            if (!await _context.BookSizes.AnyAsync(b => b.Id == book.BookSizeId && !b.IsDeleted))
                throw new ArgumentException("Invalid or deleted BookSizeId");

            if (!await _context.CoverTypes.AnyAsync(c => c.Id == book.CoverTypeId && !c.IsDeleted))
                throw new ArgumentException("Invalid or deleted CoverTypeId");

            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book?> UpdateBookAsync(UpdateBookDto bookDto, int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
                return null;

            if (!await _context.Publications.AnyAsync(p => p.Id == bookDto.PublicationId && !p.IsDeleted))
                throw new ArgumentException("Invalid or deleted publicationId");

            if (!await _context.BookSizes.AnyAsync(b => b.Id == bookDto.BookSizeId && !b.IsDeleted))
                throw new ArgumentException("Invalid or deleted BookSizeId");

            if (!await _context.CoverTypes.AnyAsync(c => c.Id == bookDto.CoverTypeId && !c.IsDeleted))
                throw new ArgumentException("Invalid or deleted CoverTypeId");

            book.Title = bookDto.Title;
            book.Author = bookDto.Author;
            book.Translator = bookDto.Translator;
            book.Description = bookDto.Description;
            book.ImageUrl = bookDto.ImageUrl;
            book.Price = bookDto.Price;
            book.Quantity = bookDto.Quantity;
            book.PageCount = bookDto.PageCount;
            book.PrintSeries = bookDto.PrintSeries;
            book.PublicationId = bookDto.PublicationId;
            book.BookSizeId = bookDto.BookSizeId;
            book.CoverTypeId = bookDto.CoverTypeId;

            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book?> DeleteBookAsync(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
                return null;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return book;
        }
    }
}
