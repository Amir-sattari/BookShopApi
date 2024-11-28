using BookShopApi.Data;
using BookShopApi.Dtos.Bookmark;
using BookShopApi.Models;

namespace BookShopApi.Interfaces
{
    public interface IBookmarkRepository
    {
        Task AddBookmarkAsync(CreateBookmarkDto bookmarkDto);
        Task DeleteBookmarkAsync(string userId);
        Task<IEnumerable<Book>> GetBookmarkedBooksByUserIdAsync(string userId);
        Task<Book?> GetBookmarkedBookByBookIdAsync(int bookId);
       
    }
}
