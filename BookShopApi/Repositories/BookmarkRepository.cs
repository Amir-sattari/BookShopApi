using BookShopApi.Data;
using BookShopApi.Dtos.Bookmark;
using BookShopApi.Interfaces;
using BookShopApi.Mappers;
using BookShopApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShopApi.Repositories
{
    public class BookmarkRepository : IBookmarkRepository
    {
        private readonly ApplicationDbContext _context;

        public BookmarkRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task AddBookmarkAsync(CreateBookmarkDto bookmarkDto)
        {
            var existingBookmark = await _context.Bookmarks.FirstOrDefaultAsync(bk => bk.UserId == bookmarkDto.UserId && bk.BookId == bookmarkDto.BookId);
            if (existingBookmark != null)
                throw new Exception("Bookmark already exists");
            
            var bookmark = bookmarkDto.ToBookmarkFromCreateDto();
            await _context.Bookmarks.AddAsync(bookmark);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBookmarkAsync(string userId)
        {
            var existingBookmark = await _context.Bookmarks.FirstOrDefaultAsync(bk => bk.UserId == userId);
            if (existingBookmark == null)
                throw new Exception("Bookmark Not Found");

            _context.Bookmarks.Remove(existingBookmark);
            await _context.SaveChangesAsync();
        }

        public async Task<Book?> GetBookmarkedBookByBookIdAsync(int bookId)
        {
            return await _context.Bookmarks.Where(bk => bk.BookId == bookId).Include(b => b.Book.BookCategories).ThenInclude(b => b.Category)
                                           .Select(bk => bk.Book).FirstAsync();
        }

        public async Task<IEnumerable<Book>> GetBookmarkedBooksByUserIdAsync(string userId)
        {
            return await _context.Bookmarks.Where(bk => bk.UserId == userId).Include(b => b.Book.BookCategories).ThenInclude(b => b.Category)
                                           .Select(bk => bk.Book).ToListAsync();
        }
    }
}
