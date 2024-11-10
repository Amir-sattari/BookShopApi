using BookShopApi.Dtos.BookSize;
using BookShopApi.Models;

namespace BookShopApi.Interfaces
{
    public interface IBookSizeRepository
    {
        Task<IEnumerable<BookSize>> GetAllBookSizeAsync();
        Task<BookSize?> GetBookSizeByIdAsync(int id);
        Task<BookSize> CreateBookSizeAsync(BookSize bookSize);
        Task<BookSize?> UpdateBookSizeAsync(UpdateBookSizeDto bookSize, int id);
        Task<BookSize?> DeleteBookSizeAsync(int id);
    }
}
