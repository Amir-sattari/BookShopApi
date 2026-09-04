using BookShopApi.Dtos.Book;
using BookShopApi.Dtos.Common;
using BookShopApi.Models;

namespace BookShopApi.Interfaces
{
    public interface IBookRepository
    {
        Task<PagedResult<Book>> GetBooksAsync(PagedQuery query);
        Task<Book?> GetBookByIdAsync(int id);
        Task<IEnumerable<Book>> GetBooksByCategoryId(int id);
        Task<Book> CreateBookAsync(CreateBookDto book);
        Task<Book?> UpdateBookAsync(UpdateBookDto bookDto, int id);
        Task<Book?> DeleteBookAsync(int id);
    }
}
