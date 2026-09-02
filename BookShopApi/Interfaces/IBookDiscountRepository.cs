using BookShopApi.Dtos.BookDiscount;
using BookShopApi.Models;

namespace BookShopApi.Interfaces
{
    public interface IBookDiscountRepository
    {
        Task<IEnumerable<BookDiscount>> GetByBookIdAsync(int bookId);
        Task<BookDiscount?> GetByIdAsync(int bookId, int id);
        Task<BookDiscount> CreateAsync(int bookId, CreateBookDiscountDto dto);
        Task<BookDiscount?> UpdateAsync(int bookId, int id, UpdateBookDiscountDto dto);
        Task<BookDiscount?> DeactivateAsync(int bookId, int id);
    }
}
