using BookShopApi.Dtos.ShoppingCart;
using BookShopApi.Models;

namespace BookShopApi.Interfaces
{
    public interface IShoppingCartRepository
    {
        Task AddToCartAsync(CreateShoppingCartDto cartDto);
        Task AddMultipleItemsToCartAsync(CreateMultipleShoppingCartItemsDto cartDto);
        Task<ShoppingCart> IncrementCartItemQuantityAsync(UpdateShoppingCartDto cartDto);
        Task<ShoppingCart> DecrementCartItemQuantityAsync(UpdateShoppingCartDto cartDto);
        Task<IEnumerable<ShoppingCart>> GetCartItemsAsync(string userId);
        Task RemovFromCartAsync(string userId, int bookId);
        Task ClearCartAsync(string userId);
    }
}
