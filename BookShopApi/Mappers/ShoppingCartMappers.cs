using BookShopApi.Dtos.ShoppingCart;
using BookShopApi.Models;

namespace BookShopApi.Mappers
{
    public static class ShoppingCartMappers
    {
        public static ShoppingCartDto ToShoppingCartDto(this ShoppingCart cart, string imageUrl)
        {
            return new ShoppingCartDto
            {
                UserId = cart.UserId,
                BookId = cart.BookId,
                Name = cart.Book.Title,
                ImageUrl = imageUrl,
                Price = cart.Book.Price,
                Quantity = cart.Quantity,
            };
        }

        public static ShoppingCart ToShoppingCartFromCreateDto(this CreateShoppingCartDto cartDto)
        {
            return new ShoppingCart
            {
                UserId = cartDto.UserId,
                BookId = cartDto.BookId,
                Quantity = 1
            };
        }
    }
}
