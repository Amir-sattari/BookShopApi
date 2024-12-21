using BookShopApi.Dtos.Discount;
using BookShopApi.Models;

namespace BookShopApi.Interfaces
{
    public interface IDiscountRepository
    {
        Task<ICollection<Discount>> GetAllDiscountsAsync();
        Task<ICollection<Discount>> GetAllActiveDiscountsAsync();
        Task<Discount?> GetSingleActiveDiscountByNameAsync(string discountName);
        Task<Discount?> GetDiscountByIdAsync(int id);
        Task<Discount?> GetDiscountByNameAsync(string discountName);
        Task<Discount> CreateDiscountAsync(CreateDiscountDto discountDto);
        Task<Discount?> UpdateDiscountAsync(UpdateDiscountDto discountDto, int id);
        Task<Discount?> DeleteDiscountByIdAsync(int id);
    }
}
