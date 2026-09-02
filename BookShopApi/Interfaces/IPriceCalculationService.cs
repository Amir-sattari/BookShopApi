using BookShopApi.Dtos.ShoppingCart;
using BookShopApi.Models;

namespace BookShopApi.Interfaces
{
    public interface IPriceCalculationService
    {
        decimal GetBookEffectivePrice(Book book);
        bool BookHasActiveDiscount(Book book);
        CartPriceResult CalculateCartTotal(IEnumerable<CartPriceItem> items, Coupon? coupon);
        Task<CartPriceResult> CalculateCartTotalAsync(IEnumerable<CartItemDto> items, string? couponCode, string userId);
    }

    public class CartPriceItem
    {
        public Book Book { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
