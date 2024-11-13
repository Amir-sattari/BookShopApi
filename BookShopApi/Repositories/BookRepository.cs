using BookShopApi.Data;
using BookShopApi.Dtos.Book;
using BookShopApi.Interfaces;
using BookShopApi.Mappers;
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
            return await _context.Books.Include(b => b.BookCategories).ThenInclude(bc => bc.Category).ToListAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _context.Books.Include(b => b.BookCategories).ThenInclude(bc => bc.Category).FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Book> CreateBookAsync(CreateBookDto bookDto)
        {
            await ValidateBookDependenciesAsync(bookDto);

            var bookModel = bookDto.SetDataToBookFromCreateDto();

            await AddCategoriesToBookAsync(bookModel, bookDto.CategoryIds);

            await _context.Books.AddAsync(bookModel);
            await _context.SaveChangesAsync();
            return bookModel;
        }

        public async Task<Book?> UpdateBookAsync(UpdateBookDto bookDto, int id)
        {
            await ValidateBookDependenciesAsync(bookDto);

            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
                return null;

            book.SetDataToBookFromUpdateDto(bookDto);
            await UpdateBookCategoriesAsync(book, bookDto.CategoryIds);

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

        // Private Methods

        private async Task UpdateBookCategoriesAsync(Book book, List<int> newCategoryIds)
        {
            var categories = await _context.BookCategories.Where(bc => bc.BookId == book.Id).ToListAsync();
            _context.BookCategories.RemoveRange(categories);
            await _context.SaveChangesAsync();
            await AddCategoriesToBookAsync(book, newCategoryIds);
        }

        private async Task AddCategoriesToBookAsync(Book book, List<int> categoryIds)
        {
            var categories = await _context.Categories.Where(c => categoryIds.Contains(c.Id) && !c.IsDeleted).ToListAsync();

            foreach (var category in categories)
            {
                book.BookCategories.Add(new BookCategory
                {
                    Book = book,
                    Category = category
                });
            }
        }

        private async Task ValidateBookDependenciesAsync(IBookDependenciesDto dto)
        {
            if (!await _context.Categories.AnyAsync(c => dto.CategoryIds.Contains(c.Id)))
                throw new ArgumentException("Invalid or Deleted CategoryId");

            if (!await _context.Publications.AnyAsync(p => p.Id == dto.PublicationId && !p.IsDeleted))
                throw new ArgumentException("Invalid or deleted publicationId");

            if (!await _context.BookSizes.AnyAsync(b => b.Id == dto.BookSizeId && !b.IsDeleted))
                throw new ArgumentException("Invalid or deleted BookSizeId");

            if (!await _context.CoverTypes.AnyAsync(c => c.Id == dto.CoverTypeId && !c.IsDeleted))
                throw new ArgumentException("Invalid or deleted CoverTypeId");
        }
    }
}
