using BookShopApi.Dtos.Book;
using BookShopApi.Models;

namespace BookShopApi.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book?> GetBookByIdAsync(int id);
        Task<Book> CreateBookAsync(Book book);
        Task<Book?> UpdateBookAsync(UpdateBookDto bookDto, int id);
        Task<Book?> DeleteBookAsync(int id);
    }
}
