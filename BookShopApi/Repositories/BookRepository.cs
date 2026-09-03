using BookShopApi.Data;
using BookShopApi.Dtos.Book;
using BookShopApi.Helpers;
using BookShopApi.Interfaces;
using BookShopApi.Mappers;
using BookShopApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShopApi.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<BookRepository> _logger;

        public BookRepository(
            ApplicationDbContext applicationDbContext,
            IFileService fileService,
            IServiceScopeFactory scopeFactory,
            ILogger<BookRepository> logger)
        {
            _context = applicationDbContext;
            _fileService = fileService;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _context.Books
                .Include(b => b.BookCategories).ThenInclude(bc => bc.Category)
                .Include(b => b.BookDiscounts)
                .ToListAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _context.Books
                .Include(b => b.BookCategories).ThenInclude(bc => bc.Category)
                .Include(b => b.BookDiscounts)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Book>> GetBooksByCategoryId(int categoryId)
        {
            if (!await IsCategoryExist(categoryId))
                throw new ArgumentException("Category with the specified ID does not exist.");

            return await _context.Books
                .Where(b => b.BookCategories.Any(bc => bc.CategoryId == categoryId))
                .Include(b => b.BookCategories).ThenInclude(bc => bc.Category)
                .Include(b => b.BookDiscounts)
                .ToListAsync();
        }

        public async Task<Book> CreateBookAsync(CreateBookDto bookDto)
        {
            await ValidateBookDependenciesAsync(bookDto);

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var imageUrl = await _fileService.SaveFileAsync(bookDto.ImageFile, allowedExtensions, "Book");

            var bookModel = bookDto.SetDataToBookFromCreateDto(imageUrl);

            await AddCategoriesToBookAsync(bookModel, bookDto.CategoryIds);

            await _context.Books.AddAsync(bookModel);
            await _context.SaveChangesAsync();
            await SyncBookDiscountAsync(bookModel.Id, bookDto.DiscountPercentage);
            await _context.SaveChangesAsync();
            return bookModel;
        }

        public async Task<Book?> UpdateBookAsync(UpdateBookDto bookDto, int id)
        {
            await ValidateBookDependenciesAsync(bookDto);

            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
                return null;

            var previousQuantity = book.Quantity;

            if (bookDto.ImageFile != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                string folderName = "Book";
                if (!string.IsNullOrEmpty(book.ImageUrl))
                    _fileService.DeleteFile(book.ImageUrl, folderName);

                book.ImageUrl = await _fileService.SaveFileAsync(bookDto.ImageFile, allowedExtensions, "Book");
            }

            book.SetDataToBookFromUpdateDto(bookDto);

            await UpdateBookCategoriesAsync(book, bookDto.CategoryIds);
            await SyncBookDiscountAsync(book.Id, bookDto.DiscountPercentage);

            await _context.SaveChangesAsync();
            NotifyIfRestocked(book.Id, previousQuantity, book.Quantity);
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

        private async Task SyncBookDiscountAsync(int bookId, decimal percentage)
        {
            var activeDiscounts = await _context.BookDiscounts
                .Where(d => d.BookId == bookId && d.IsActive)
                .ToListAsync();

            var now = DateTime.UtcNow;
            var currentlyActive = activeDiscounts
                .Where(d => d.IsCurrentlyActive(now) && d.Percentage > 0)
                .OrderByDescending(d => d.Id)
                .FirstOrDefault();

            if (percentage <= 0)
            {
                foreach (var discount in activeDiscounts)
                    discount.IsActive = false;
                return;
            }

            if (currentlyActive != null && currentlyActive.Percentage == percentage)
                return;

            foreach (var discount in activeDiscounts)
                discount.IsActive = false;

            await _context.BookDiscounts.AddAsync(new BookDiscount
            {
                BookId = bookId,
                Percentage = percentage,
                IsActive = true
            });
        }

        private async Task<bool> IsCategoryExist(int id)
        {
            return await _context.Categories.AnyAsync(c => c.Id == id);
        }

        private void NotifyIfRestocked(int bookId, int previousQuantity, int newQuantity)
        {
            if (previousQuantity != 0 || newQuantity <= 0)
                return;

            // Fire-and-forget so the admin's request doesn't wait on notification processing.
            // Temporary until a real background job/queue (e.g. Hangfire) is introduced.
            // A new DI scope is required because the request scope (and DbContext) is disposed after the HTTP response.
            _ = NotifySubscribersInBackgroundAsync(bookId);
        }

        private async Task NotifySubscribersInBackgroundAsync(int bookId)
        {
            try
            {
                await using var scope = _scopeFactory.CreateAsyncScope();
                var stockNotificationService = scope.ServiceProvider.GetRequiredService<IStockNotificationService>();
                await stockNotificationService.NotifySubscribersForBookAsync(bookId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process stock notifications for book {BookId}.", bookId);
            }
        }
    }
}
